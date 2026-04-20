using InteractHub.Core.DTOs.PostReports;
using InteractHub.Core.Entities;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InteractHub.Infrastructure.Services
{
    public class PostReportService : IPostReportService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public PostReportService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task CreateReportAsync(long userId, CreatePostReportRequestDto dto)
        {
            
            var post = await _context.Posts
                .FirstOrDefaultAsync(p => p.PostId == dto.PostId && !p.Delflg);

            if (post == null) throw new KeyNotFoundException();

            var report = new PostReport
            {
                PostId = dto.PostId,
                ReporterId = userId,
                Reason = dto.Reason,
                Description = dto.Description,
                Status = "Pending",
                Delflg = false,
                RegDatetime = DateTime.UtcNow
            };

            post.IsReported = true;
            post.ReportCount += 1;

            _context.PostReports.Add(report);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReportStatusByPostAsync(long adminId, long postId, UpdatePostReportRequestDto dto)
        {
            var relatedReports = await _context.PostReports
                .Where(r => r.PostId == postId && !r.Delflg)
                .ToListAsync();

            if (!relatedReports.Any()) throw new KeyNotFoundException();

            
            var post = await _context.Posts
                .FirstOrDefaultAsync(p => p.PostId == postId && !p.Delflg);

            DateTime now = DateTime.UtcNow;

            if (dto.Status == "Resolved" && post != null)
            {
                switch (dto.ActionTaken)
                {
                    case "PostRemoved":
                        
                        post.Delflg = true;
                        post.UpdDatetime = now;
                        break;

                    case "UserBanned":
                        var author = await _userManager.FindByIdAsync(post.UserId.ToString());
                        if (author != null)
                        {
                            author.IsActive = false;
                            author.LockoutEnd = DateTimeOffset.MaxValue;
                            await _userManager.UpdateAsync(author);

                           
                            post.Delflg = true;
                            post.UpdDatetime = now;

                            
                            await _context.Posts
                                .Where(p => p.UserId == post.UserId && !p.Delflg)
                                .ExecuteUpdateAsync(s => s.SetProperty(p => p.Delflg, true)
                                                          .SetProperty(p => p.UpdDatetime, now));
                        }
                        break;

                    case "UserWarned":
                    
                        break;
                }
            }

            foreach (var report in relatedReports)
            {
                if (report.Status == "Pending" || report.Status == "Reviewing")
                {
                    report.Status = dto.Status;
                    report.ReviewedById = adminId;
                    report.ActionTaken = dto.ActionTaken;
                    report.ReviewNote = dto.ReviewNote;
                    report.ReviewDatetime = now;
                    report.UpdDatetime = now;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponseDto<PostReportGroupSummaryResponseDto>> GetAllGroupedReportsAsync(int page, int pageSize)
        {
          
            var query = _context.PostReports
                .AsNoTracking()
                .Where(r => !r.Delflg) 
                .GroupBy(r => r.PostId);

            var totalCount = await query.CountAsync();

            var reports = await query
                .Select(g => new PostReportGroupSummaryResponseDto
                {
                    PostId = g.Key,
                    TotalReports = g.Count(),
                    
                    Status = g.OrderByDescending(r => r.RegDatetime)
                              .Select(r => r.Status)
                              .FirstOrDefault() ?? string.Empty,
                    LatestReportDatetime = g.Max(r => r.RegDatetime),
                   
                    ReviewedById = g.OrderByDescending(r => r.RegDatetime).Select(r => r.ReviewedById).FirstOrDefault()
                })
                .OrderByDescending(r => r.LatestReportDatetime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponseDto<PostReportGroupSummaryResponseDto>
            {
                Items = reports,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResponseDto<PostReportsByPostDetailResponseDto>> GetReportsByPostIdAsync(long postId, int page, int pageSize)
        {
            var query = _context.PostReports
                .AsNoTracking()
                .Where(r => r.PostId == postId && !r.Delflg); 

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(r => r.RegDatetime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new PostReportsByPostDetailResponseDto
                {
                    ReportId = r.ReportId,
                    PostId = r.PostId,
                    ReporterId = r.ReporterId,
                    Reason = r.Reason,
                    Description = r.Description,
                    Status = r.Status,
                    ReviewedById = r.ReviewedById,
                    ReviewNote = r.ReviewNote,
                    ActionTaken = r.ActionTaken,
                    Delflg = r.Delflg,
                    RegDatetime = r.RegDatetime,
                    UpdDatetime = r.UpdDatetime
                }).ToListAsync();

            return new PagedResponseDto<PostReportsByPostDetailResponseDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PostReportsByPostDetailResponseDto?> GetReportByIdAsync(long reportId)
        {
            var r = await _context.PostReports
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReportId == reportId && !x.Delflg);

            if (r == null) return null;

            return new PostReportsByPostDetailResponseDto
            {
                ReportId = r.ReportId,
                PostId = r.PostId,
                ReporterId = r.ReporterId,
                Reason = r.Reason,
                Description = r.Description,
                Status = r.Status,
                ReviewedById = r.ReviewedById,
                ReviewNote = r.ReviewNote,
                ActionTaken = r.ActionTaken,
                Delflg = r.Delflg,
                RegDatetime = r.RegDatetime,
                UpdDatetime = r.UpdDatetime
            };
        }

        public async Task DeleteReportsByPostAsync(long postId)
        {
            
            var reports = await _context.PostReports
                .Where(r => r.PostId == postId && !r.Delflg)
                .ToListAsync();

           
            if (reports == null || !reports.Any()) 
                throw new KeyNotFoundException();

            DateTime now = DateTime.UtcNow;

           
            foreach (var report in reports)
            {
                report.Delflg = true;
                report.UpdDatetime = now;
            }

            
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                post.IsReported = false;
                post.ReportCount = 0;
                post.UpdDatetime = now;
            }

            await _context.SaveChangesAsync();
        }
    }
}
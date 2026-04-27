using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using InteractHub.Core.DTOs.Uploads;
using InteractHub.Core.Entities;
using InteractHub.Core.Helpers;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InteractHub.Infrastructure.Services;

public class FileUploadService : IFileUploadService
{
    private const long MaxAvatarOrCoverBytes = 5 * 1024 * 1024;
    private const long MaxPostOrStoryMediaBytes = 50 * 1024 * 1024;

    private static readonly HashSet<string> ImageContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp",
        "image/gif"
    };

    private static readonly HashSet<string> VideoContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "video/mp4",
        "video/webm",
        "video/quicktime"
    };

    private readonly AppDbContext _context;
    private readonly AzureBlobStorageSettings _storageSettings;
    private readonly BlobContainerClient _containerClient;

    public FileUploadService(
        AppDbContext context,
        IOptions<AzureBlobStorageSettings> storageOptions)
    {
        _context = context;
        _storageSettings = storageOptions.Value;

        if (string.IsNullOrWhiteSpace(_storageSettings.ConnectionString))
        {
            throw new InvalidOperationException("Azure Blob Storage connection string is not configured.");
        }

        if (string.IsNullOrWhiteSpace(_storageSettings.ContainerName))
        {
            throw new InvalidOperationException("Azure Blob Storage container name is not configured.");
        }

        var serviceClient = new BlobServiceClient(_storageSettings.ConnectionString);
        _containerClient = serviceClient.GetBlobContainerClient(_storageSettings.ContainerName);
    }

    public async Task<FileUploadResponseDto> UploadAvatarAsync(
        long currentUserId,
        UploadAvatarRequestDto dto,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize)
    {
        EnsureUserOwnership(currentUserId, dto.UserId);
        var mediaType = ValidateContentTypeAndSize(contentType, fileSize, MaxAvatarOrCoverBytes, onlyImage: true);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId && !u.Delflg);
        if (user == null)
        {
            throw new KeyNotFoundException("Không tìm thấy người dùng.");
        }

        var uploaded = await UploadToBlobAsync(
            category: "avatars",
            ownerId: dto.UserId,
            fileStream: fileStream,
            fileName: fileName,
            contentType: contentType);

        user.AvatarUrl = uploaded.Url;
        user.UpDatetime = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new FileUploadResponseDto
        {
            FileCategory = "Avatar",
            FileName = uploaded.FileName,
            ContentType = contentType,
            FileSizeBytes = fileSize,
            MediaType = mediaType,
            Url = uploaded.Url,
            UserId = dto.UserId
        };
    }

    public async Task<FileUploadResponseDto> UploadCoverPhotoAsync(
        long currentUserId,
        UploadCoverPhotoRequestDto dto,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize)
    {
        EnsureUserOwnership(currentUserId, dto.UserId);
        var mediaType = ValidateContentTypeAndSize(contentType, fileSize, MaxAvatarOrCoverBytes, onlyImage: true);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId && !u.Delflg);
        if (user == null)
        {
            throw new KeyNotFoundException("Không tìm thấy người dùng.");
        }

        var uploaded = await UploadToBlobAsync(
            category: "covers",
            ownerId: dto.UserId,
            fileStream: fileStream,
            fileName: fileName,
            contentType: contentType);

        user.CoverPhotoUrl = uploaded.Url;
        user.UpDatetime = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new FileUploadResponseDto
        {
            FileCategory = "CoverPhoto",
            FileName = uploaded.FileName,
            ContentType = contentType,
            FileSizeBytes = fileSize,
            MediaType = mediaType,
            Url = uploaded.Url,
            UserId = dto.UserId
        };
    }

    public async Task<FileUploadResponseDto> UploadPostMediaAsync(
        long currentUserId,
        UploadPostMediaRequestDto dto,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize)
    {
        var mediaType = ValidateContentTypeAndSize(contentType, fileSize, MaxPostOrStoryMediaBytes, onlyImage: false);

        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.PostId == dto.PostId && p.UserId == currentUserId && !p.Delflg);

        if (post == null)
        {
            throw new KeyNotFoundException("Không tìm thấy bài viết hoặc bạn không có quyền upload media.");
        }

        var uploaded = await UploadToBlobAsync(
            category: "post-media",
            ownerId: currentUserId,
            fileStream: fileStream,
            fileName: fileName,
            contentType: contentType);

        var postMedia = new PostMedia
        {
            PostId = post.PostId,
            MediaUrl = uploaded.Url,
            MediaType = mediaType,
            SortOrder = dto.SortOrder,
            FileName = uploaded.FileName,
            FileSizeKb = (int)Math.Ceiling(fileSize / 1024d),
            RegDatetime = DateTime.UtcNow,
            ProcessingStatus = "Ready"
        };

        _context.PostMedias.Add(postMedia);
        await _context.SaveChangesAsync();

        return new FileUploadResponseDto
        {
            FileCategory = "PostMedia",
            FileName = uploaded.FileName,
            ContentType = contentType,
            FileSizeBytes = fileSize,
            MediaType = mediaType,
            Url = uploaded.Url,
            PostId = post.PostId,
            UserId = currentUserId
        };
    }

    public async Task<FileUploadResponseDto> UploadStoryMediaAsync(
        long currentUserId,
        UploadStoryMediaRequestDto dto,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize)
    {
        var mediaType = ValidateContentTypeAndSize(contentType, fileSize, MaxPostOrStoryMediaBytes, onlyImage: false);

        var story = await _context.Stories
            .FirstOrDefaultAsync(s => s.StoryId == dto.StoryId && s.UserId == currentUserId && !s.Delflg);

        if (story == null)
        {
            throw new KeyNotFoundException("Không tìm thấy story hoặc bạn không có quyền upload media.");
        }

        var uploaded = await UploadToBlobAsync(
            category: "story-media",
            ownerId: currentUserId,
            fileStream: fileStream,
            fileName: fileName,
            contentType: contentType);

        story.MediaUrl = uploaded.Url;
        story.MediaType = mediaType;
        await _context.SaveChangesAsync();

        return new FileUploadResponseDto
        {
            FileCategory = "StoryMedia",
            FileName = uploaded.FileName,
            ContentType = contentType,
            FileSizeBytes = fileSize,
            MediaType = mediaType,
            Url = uploaded.Url,
            StoryId = story.StoryId,
            UserId = currentUserId
        };
    }

    private static void EnsureUserOwnership(long currentUserId, long targetUserId)
    {
        if (currentUserId != targetUserId)
        {
            throw new UnauthorizedAccessException("Bạn không có quyền thao tác tài nguyên của người dùng khác.");
        }
    }

    private static string ValidateContentTypeAndSize(
        string contentType,
        long fileSize,
        long maxSize,
        bool onlyImage)
    {
        if (fileSize <= 0)
        {
            throw new ArgumentException("File upload không hợp lệ.");
        }

        if (fileSize > maxSize)
        {
            throw new ArgumentException($"Kích thước file vượt quá giới hạn {maxSize / (1024 * 1024)}MB.");
        }

        if (ImageContentTypes.Contains(contentType))
        {
            return "Image";
        }

        if (!onlyImage && VideoContentTypes.Contains(contentType))
        {
            return "Video";
        }

        throw new ArgumentException(onlyImage
            ? "Chỉ chấp nhận file ảnh (jpeg, png, webp, gif)."
            : "Chỉ chấp nhận file ảnh/video hợp lệ.");
    }

    private async Task<(string Url, string FileName)> UploadToBlobAsync(
        string category,
        long ownerId,
        Stream fileStream,
        string fileName,
        string contentType)
    {
        await _containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

        var extension = Path.GetExtension(fileName);
        var safeExtension = string.IsNullOrWhiteSpace(extension)
            ? GuessExtensionFromContentType(contentType)
            : extension.ToLowerInvariant();

        var storedFileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}{safeExtension}";
        var blobPath = $"{category}/{ownerId}/{storedFileName}";

        var blobClient = _containerClient.GetBlobClient(blobPath);
        fileStream.Position = 0;

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            }
        };

        await blobClient.UploadAsync(fileStream, uploadOptions);

        return (blobClient.Uri.ToString(), storedFileName);
    }

    private static string GuessExtensionFromContentType(string contentType)
    {
        return contentType.ToLowerInvariant() switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            "image/gif" => ".gif",
            "video/mp4" => ".mp4",
            "video/webm" => ".webm",
            "video/quicktime" => ".mov",
            _ => ".bin"
        };
    }
}

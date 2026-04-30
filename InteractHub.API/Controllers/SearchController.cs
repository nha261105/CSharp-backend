using System.Security.Claims;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InteractHub.API.Controllers;

[ApiController]
[Route("api/search")]
[Authorize]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    private long? TryGetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (claim != null && long.TryParse(claim.Value, out var id)) return id;
        return null;
    }

    /// <summary>
    /// Tìm kiếm tổng hợp users + posts theo keyword.
    /// GET /api/search?q=keyword&amp;page=1&amp;pageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GlobalSearch(
        [FromQuery] string q,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống." });

        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;

        var currentUserId = TryGetCurrentUserId();
        var result = await _searchService.GlobalSearchAsync(currentUserId, q, page, pageSize);
        return Ok(result);
    }
}

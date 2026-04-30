namespace InteractHub.Core.DTOs.Search;

public class GlobalSearchResponseDto
{
    public string Keyword { get; set; } = string.Empty;
    public List<SearchUserResultDto> Users { get; set; } = new();
    public List<SearchPostResultDto> Posts { get; set; } = new();
}

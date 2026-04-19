namespace InteractHub.Core.DTOs.Posts;

using System.ComponentModel.DataAnnotations;

public class UpdatePostRequestDto
{
    [MaxLength(50000)]
    public string? Content {get; set;}
    public string? ContentFormat {get; set;}
    public string? Visibility {get; set;}
    public string? LocationName {get; set;}
}
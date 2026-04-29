namespace InteractHub.Core.DTOs.Posts;

public class CommentReactionDetailResponseDto
{
    public string ReactionType { get; set; } = string.Empty;
    
    public int Count { get; set; }

    public List<UserSummaryResponseDto> Users { get; set; } = new();
}
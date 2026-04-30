namespace InteractHub.Core.DTOs.Posts;

public class PostReactionRequestDto
{
    /// <summary>
    /// Loại cảm xúc: Like | Love | Haha | Wow | Sad | Angry
    /// </summary>
    public string ReactionType { get; set; } = "Like";
}

using Microsoft.AspNetCore.Identity;
namespace InteractHub.Core.Entities;

public class User : IdentityUser<long>
{
    public string Fullname {get; set;} = string.Empty;
    public string? Gender {get; set;}
    public DateTime? DateOfBirth {get; set;}
    public string? AvatarUrl {get; set;}
    public string? CoverPhotoUrl { get; set; }
    public string? Bio {get; set;}
    public string? WebsiteUrl {get; set;}
    public string? Location {get; set;}
    public bool IsActive {get; set;} = true;
    public bool IsPrivateAccount {get; set;} = false;
    public DateTime? LastLoginDateTime {get; set;}
    public bool Delflg {get; set;} = false;
    public DateTime RegDateTime {get; set;} = DateTime.UtcNow;
    public DateTime? UpDatetime {get; set;}
    public virtual UserProfile? Profile {get; set;}
    public virtual ICollection<Post> Posts {get; set; } = new List<Post>();
    public virtual ICollection<Comment> Comments {get; set;} = new List<Comment>();
    public virtual ICollection<PostLike> PostLikes {get; set;} = new List<PostLike>();
    public virtual ICollection<PostShare> PostShares {get; set;} = new List<PostShare>();
    public virtual ICollection<Story> Stories {get; set;} = new List<Story>();
    public virtual ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
    public virtual ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
    public virtual ICollection<Friendship> SentFriendships { get; set; } = new List<Friendship>();
    public virtual ICollection<Friendship> ReceivedFriendships { get; set; } = new List<Friendship>();

}
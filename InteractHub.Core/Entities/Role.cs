using Microsoft.AspNetCore.Identity;

namespace InteractHub.Core.Entities;

public class Role : IdentityRole<int>
{
    public string? Description { get; set; }
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;
}

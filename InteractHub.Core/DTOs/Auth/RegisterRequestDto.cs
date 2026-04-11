namespace InteractHub.Core.DTOs.Auth;

public class RegisterRequestDto
{
    public string Username {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;
    public string Fullname {get; set;} = string.Empty;
    public string? PhoneNumber {get; set;}
}
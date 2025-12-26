namespace SaaSPlatform.Application.DTOs.Auth;

public class AuthResponseDto
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? Expiration { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IList<string>? Roles { get; set; }
    public string? Message { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

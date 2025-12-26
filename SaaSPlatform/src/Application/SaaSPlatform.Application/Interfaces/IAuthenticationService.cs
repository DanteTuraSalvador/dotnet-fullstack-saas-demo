using SaaSPlatform.Application.DTOs.Auth;

namespace SaaSPlatform.Application.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto request);
    Task<AuthResponseDto> LoginAsync(LoginDto request);
    Task<AuthResponseDto> RefreshTokenAsync(string token, string refreshToken);
    Task<bool> RevokeTokenAsync(string userId);
}

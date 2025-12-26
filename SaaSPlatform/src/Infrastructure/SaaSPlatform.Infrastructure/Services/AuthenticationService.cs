using Microsoft.AspNetCore.Identity;
using SaaSPlatform.Application.DTOs.Auth;
using SaaSPlatform.Application.Interfaces;
using SaaSPlatform.Infrastructure.Identity;

namespace SaaSPlatform.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "User with this email already exists",
                Errors = new[] { "Email already registered" }
            };
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CompanyName = request.CompanyName,
            EmailConfirmed = true // For demo purposes, auto-confirm email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Registration failed",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        // Assign default role
        await _userManager.AddToRoleAsync(user, AppRoles.Client);

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateAccessToken(user.Id, user.Email!, roles);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        // Store refresh token (in production, store in database)
        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            Success = true,
            Token = token,
            RefreshToken = refreshToken,
            Expiration = _jwtTokenService.GetAccessTokenExpiration(),
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToList(),
            Message = "Registration successful"
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !user.IsActive)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid email or password",
                Errors = new[] { "Invalid credentials" }
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Account is locked out. Please try again later.",
                Errors = new[] { "Account locked" }
            };
        }

        if (!result.Succeeded)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid email or password",
                Errors = new[] { "Invalid credentials" }
            };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateAccessToken(user.Id, user.Email!, roles);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            Success = true,
            Token = token,
            RefreshToken = refreshToken,
            Expiration = _jwtTokenService.GetAccessTokenExpiration(),
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToList(),
            Message = "Login successful"
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string token, string refreshToken)
    {
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(token);
        if (principal == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid token",
                Errors = new[] { "Token validation failed" }
            };
        }

        var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid token",
                Errors = new[] { "User ID not found in token" }
            };
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || !user.IsActive)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "User not found or inactive",
                Errors = new[] { "Invalid user" }
            };
        }

        // In production, validate refresh token from database
        var roles = await _userManager.GetRolesAsync(user);
        var newToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email!, roles);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        return new AuthResponseDto
        {
            Success = true,
            Token = newToken,
            RefreshToken = newRefreshToken,
            Expiration = _jwtTokenService.GetAccessTokenExpiration(),
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToList(),
            Message = "Token refreshed successfully"
        };
    }

    public async Task<bool> RevokeTokenAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        // In production, invalidate refresh token in database
        return true;
    }
}

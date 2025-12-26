using Microsoft.AspNetCore.Identity;

namespace SaaSPlatform.Infrastructure.Identity;

public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ApplicationRole() : base() { }

    public ApplicationRole(string roleName) : base(roleName) { }

    public ApplicationRole(string roleName, string description) : base(roleName)
    {
        Description = description;
    }
}

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Client = "Client";
}

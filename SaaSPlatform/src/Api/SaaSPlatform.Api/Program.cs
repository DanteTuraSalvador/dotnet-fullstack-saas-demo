using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SaaSPlatform.Application.Interfaces;
using SaaSPlatform.Application.Services;
using SaaSPlatform.Infrastructure.Data;
using SaaSPlatform.Infrastructure.Identity;
using SaaSPlatform.Infrastructure.Repositories;
using SaaSPlatform.Infrastructure.Services;
using Serilog;

namespace SaaSPlatform.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build())
            .Enrich.FromLogContext()
            .CreateLogger();

        try
        {
            Log.Information("Starting SaaSPlatform API...");

            var builder = WebApplication.CreateBuilder(args);

            // Use Serilog
            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddOpenApi();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add Health Checks
            builder.Services.AddHealthChecks()
                .AddSqlServer(
                    connectionString!,
                    name: "sqlserver",
                    tags: new[] { "db", "sql", "sqlserver" })
                .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(),
                    tags: new[] { "self" });

            // Add Identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Add JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();

            // Add SignalR
            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontendClients", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:4200",
                            "https://localhost:4200",
                            "http://localhost:4300",
                            "https://localhost:4300",
                            "http://localhost:4400",
                            "https://localhost:4400",
                            "http://localhost:44300",
                            "https://localhost:44300",
                            "http://localhost:44400",
                            "https://localhost:44400",
                            "http://localhost:4500",
                            "https://localhost:4500",
                            "http://localhost:44500",
                            "https://localhost:44500",
                            "http://localhost:5173",
                            "https://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            // Add controllers
            builder.Services.AddControllers();

            // Add application services
            builder.Services.AddScoped<IClientSubscriptionRepository, ClientSubscriptionRepository>();
            builder.Services.AddScoped<IClientSubscriptionService, ClientSubscriptionService>();
            builder.Services.AddScoped<IAzureDeploymentService, AzureDeploymentService>();

            // Add authentication services
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            var app = builder.Build();

            // Seed roles on startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                SeedRoles(services).GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Add request logging
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontendClients");

            app.UseAuthentication();
            app.UseAuthorization();

            // Map Health Check endpoints
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("db"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("self"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapControllers();

            // Map SignalR hubs
            app.MapHub<DeploymentHub>("/hubs/deployment");

            Log.Information("SaaSPlatform API started successfully");

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        string[] roleNames = { AppRoles.Admin, AppRoles.Client };

        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new ApplicationRole(roleName, $"{roleName} role"));
            }
        }
    }
}

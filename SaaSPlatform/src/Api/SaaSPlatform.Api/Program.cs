using Microsoft.EntityFrameworkCore;
using SaaSPlatform.Application.Interfaces;
using SaaSPlatform.Application.Services;
using SaaSPlatform.Infrastructure.Data;
using SaaSPlatform.Infrastructure.Repositories;
using SaaSPlatform.Infrastructure.Services;

namespace SaaSPlatform.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

// Add DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
                        "https://localhost:44400")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Add controllers
        builder.Services.AddControllers();

        // Add application services
        builder.Services.AddScoped<IClientSubscriptionRepository, ClientSubscriptionRepository>();
        builder.Services.AddScoped<IClientSubscriptionService, ClientSubscriptionService>();
        builder.Services.AddScoped<IAzureDeploymentService, AzureDeploymentService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowFrontendClients");

        app.MapControllers();

        app.Run();
    }
}

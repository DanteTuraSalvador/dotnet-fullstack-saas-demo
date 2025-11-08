using Microsoft.EntityFrameworkCore;
using SaaSPlatform.Domain.Entities;

namespace SaaSPlatform.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ClientSubscription> ClientSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ClientSubscription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ContactEmail).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ContactPerson).IsRequired().HasMaxLength(200);
            entity.Property(e => e.BusinessType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.AzureResourceGroup).HasMaxLength(200);
            entity.Property(e => e.WebAppUrl).HasMaxLength(500);
        });
    }
}
using Microsoft.EntityFrameworkCore;
using SaaSPlatform.Application.Interfaces;
using SaaSPlatform.Domain.Entities;
using SaaSPlatform.Infrastructure.Data;

namespace SaaSPlatform.Infrastructure.Repositories;

public class ClientSubscriptionRepository : IClientSubscriptionRepository
{
    private readonly AppDbContext _context;

    public ClientSubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClientSubscription>> GetAllAsync()
    {
        return await _context.ClientSubscriptions.ToListAsync();
    }

    public async Task<ClientSubscription?> GetByIdAsync(int id)
    {
        return await _context.ClientSubscriptions.FindAsync(id);
    }

    public async Task<ClientSubscription> AddAsync(ClientSubscription subscription)
    {
        _context.ClientSubscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task<ClientSubscription> UpdateAsync(ClientSubscription subscription)
    {
        _context.Entry(subscription).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var subscription = await _context.ClientSubscriptions.FindAsync(id);
        if (subscription == null)
            return false;

        _context.ClientSubscriptions.Remove(subscription);
        await _context.SaveChangesAsync();
        return true;
    }
}
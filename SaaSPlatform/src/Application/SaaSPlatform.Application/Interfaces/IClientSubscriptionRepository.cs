using SaaSPlatform.Domain.Entities;

namespace SaaSPlatform.Application.Interfaces;

public interface IClientSubscriptionRepository
{
    Task<IEnumerable<ClientSubscription>> GetAllAsync();
    Task<ClientSubscription?> GetByIdAsync(int id);
    Task<ClientSubscription> AddAsync(ClientSubscription subscription);
    Task<ClientSubscription> UpdateAsync(ClientSubscription subscription);
    Task<bool> DeleteAsync(int id);
}
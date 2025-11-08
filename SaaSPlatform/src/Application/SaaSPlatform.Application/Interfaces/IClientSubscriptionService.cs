using SaaSPlatform.Application.DTOs;
using SaaSPlatform.Domain.Entities;

namespace SaaSPlatform.Application.Interfaces;

public interface IClientSubscriptionService
{
    Task<IEnumerable<SubscriptionResponseDto>> GetAllSubscriptionsAsync();
    Task<SubscriptionResponseDto?> GetSubscriptionByIdAsync(int id);
    Task<SubscriptionResponseDto> CreateSubscriptionAsync(CreateSubscriptionDto subscriptionDto);
    Task<SubscriptionResponseDto> UpdateSubscriptionStatusAsync(int id, SubscriptionStatus status);
    Task<SubscriptionResponseDto> DeploySubscriptionAsync(int id);
    Task<bool> DeleteSubscriptionAsync(int id);
}

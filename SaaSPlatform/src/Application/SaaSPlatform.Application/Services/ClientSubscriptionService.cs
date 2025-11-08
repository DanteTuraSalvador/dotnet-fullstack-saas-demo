using SaaSPlatform.Application.DTOs;
using SaaSPlatform.Application.Interfaces;
using SaaSPlatform.Domain.Entities;

namespace SaaSPlatform.Application.Services;

public class ClientSubscriptionService : IClientSubscriptionService
{
    private readonly IClientSubscriptionRepository _repository;
    private readonly IAzureDeploymentService _deploymentService;

    public ClientSubscriptionService(IClientSubscriptionRepository repository, IAzureDeploymentService deploymentService)
    {
        _repository = repository;
        _deploymentService = deploymentService;
    }

    public async Task<IEnumerable<SubscriptionResponseDto>> GetAllSubscriptionsAsync()
    {
        var subscriptions = await _repository.GetAllAsync();
        return subscriptions.Select(MapToResponse);
    }

    public async Task<SubscriptionResponseDto?> GetSubscriptionByIdAsync(int id)
    {
        var subscription = await _repository.GetByIdAsync(id);
        if (subscription == null)
            return null;

        return MapToResponse(subscription);
    }

    public async Task<SubscriptionResponseDto> CreateSubscriptionAsync(CreateSubscriptionDto subscriptionDto)
    {
        var subscription = new ClientSubscription
        {
            CompanyName = subscriptionDto.CompanyName,
            ContactEmail = subscriptionDto.ContactEmail,
            ContactPerson = subscriptionDto.ContactPerson,
            BusinessType = subscriptionDto.BusinessType,
            CreatedDate = DateTime.UtcNow,
            Status = SubscriptionStatus.Pending
        };

        var createdSubscription = await _repository.AddAsync(subscription);

        return MapToResponse(createdSubscription);
    }

    public async Task<SubscriptionResponseDto> UpdateSubscriptionStatusAsync(int id, SubscriptionStatus status)
    {
        var subscription = await _repository.GetByIdAsync(id);
        if (subscription == null)
            throw new ArgumentException("Subscription not found", nameof(id));

        subscription.Status = status;
        await _repository.UpdateAsync(subscription);

        return MapToResponse(subscription);
    }

    public async Task<SubscriptionResponseDto> DeploySubscriptionAsync(int id)
    {
        var subscription = await _repository.GetByIdAsync(id);
        if (subscription == null)
            throw new ArgumentException("Subscription not found", nameof(id));

        var deploymentResult = await _deploymentService.DeploySubscriptionAsync(subscription);

        subscription.Status = SubscriptionStatus.Deployed;
        subscription.AzureResourceGroup = deploymentResult.ResourceGroupName;
        subscription.WebAppUrl = deploymentResult.WebAppUrl;

        await _repository.UpdateAsync(subscription);

        return MapToResponse(subscription);
    }

    public async Task<bool> DeleteSubscriptionAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static SubscriptionResponseDto MapToResponse(ClientSubscription subscription)
    {
        return new SubscriptionResponseDto
        {
            Id = subscription.Id,
            CompanyName = subscription.CompanyName,
            ContactEmail = subscription.ContactEmail,
            ContactPerson = subscription.ContactPerson,
            BusinessType = subscription.BusinessType,
            CreatedDate = subscription.CreatedDate,
            SubmittedDate = subscription.CreatedDate,
            Status = subscription.Status.ToString(),
            AzureResourceGroup = subscription.AzureResourceGroup,
            WebAppUrl = subscription.WebAppUrl
        };
    }
}

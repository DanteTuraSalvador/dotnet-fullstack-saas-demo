using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSPlatform.Application.DTOs;
using SaaSPlatform.Application.Interfaces;
using SaaSPlatform.Domain.Entities;

namespace SaaSPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionsController : ControllerBase
{
    private readonly IClientSubscriptionService _subscriptionService;

    public SubscriptionsController(IClientSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    /// <summary>
    /// Get all client subscriptions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubscriptionResponseDto>>> GetAllSubscriptions()
    {
        var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
        return Ok(subscriptions);
    }

    /// <summary>
    /// Get a specific client subscription by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<SubscriptionResponseDto>> GetSubscription(int id)
    {
        var subscription = await _subscriptionService.GetSubscriptionByIdAsync(id);
        if (subscription == null)
            return NotFound();

        return Ok(subscription);
    }

    /// <summary>
    /// Create a new client subscription
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<SubscriptionResponseDto>> CreateSubscription(CreateSubscriptionDto subscriptionDto)
    {
        try
        {
            var subscription = await _subscriptionService.CreateSubscriptionAsync(subscriptionDto);
            return CreatedAtAction(nameof(GetSubscription), new { id = subscription.Id }, subscription);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update the status of a client subscription
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<ActionResult<SubscriptionResponseDto>> UpdateSubscriptionStatus(int id, [FromBody] string status)
    {
        try
        {
            if (!Enum.TryParse<SubscriptionStatus>(status, out var subscriptionStatus))
            {
                return BadRequest("Invalid status value");
            }

            var subscription = await _subscriptionService.UpdateSubscriptionStatusAsync(id, subscriptionStatus);
            return Ok(subscription);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deploy Azure resources for a subscription
    /// </summary>
    [HttpPost("{id}/deploy")]
    public async Task<ActionResult<SubscriptionResponseDto>> DeploySubscription(int id)
    {
        try
        {
            var subscription = await _subscriptionService.DeploySubscriptionAsync(id);
            return Ok(subscription);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete a client subscription
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubscription(int id)
    {
        var result = await _subscriptionService.DeleteSubscriptionAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}

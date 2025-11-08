namespace SaaSPlatform.Application.DTOs;

public class CreateSubscriptionDto
{
    public required string CompanyName { get; set; }
    public required string ContactEmail { get; set; }
    public required string ContactPerson { get; set; }
    public required string BusinessType { get; set; }
}
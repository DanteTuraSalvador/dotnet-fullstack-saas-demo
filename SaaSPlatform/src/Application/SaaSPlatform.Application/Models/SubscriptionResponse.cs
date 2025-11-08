using System.Text.Json.Serialization;

namespace SaaSPlatform.Application.Models;

public class SubscriptionResponse
{
    public int Id { get; set; }
    
    [JsonPropertyName("companyName")]
    public string CompanyName { get; set; } = string.Empty;
    
    [JsonPropertyName("contactEmail")]
    public string ContactEmail { get; set; } = string.Empty;
    
    [JsonPropertyName("contactPerson")]
    public string ContactPerson { get; set; } = string.Empty;
    
    [JsonPropertyName("businessType")]
    public string BusinessType { get; set; } = string.Empty;
    
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    
    [JsonPropertyName("submittedDate")]
    public DateTime SubmittedDate { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonPropertyName("azureResourceGroup")]
    public string? AzureResourceGroup { get; set; }
    
    [JsonPropertyName("webAppUrl")]
    public string? WebAppUrl { get; set; }
}
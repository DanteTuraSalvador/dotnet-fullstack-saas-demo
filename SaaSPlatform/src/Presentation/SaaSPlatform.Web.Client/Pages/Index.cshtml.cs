using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaaSPlatform.Application.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace SaaSPlatform.Web.Client.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    [Required]
    public string CompanyName { get; set; } = string.Empty;

    [BindProperty]
    [Required, EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string ContactPerson { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string BusinessType { get; set; } = string.Empty;

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Call API to submit subscription request
        var client = _httpClientFactory.CreateClient("ApiClient");
        
        var subscriptionRequest = new
        {
            CompanyName = CompanyName,
            ContactEmail = ContactEmail,
            ContactPerson = ContactPerson,
            BusinessType = BusinessType
        };
        
        var json = JsonSerializer.Serialize(subscriptionRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        try
        {
            var response = await client.PostAsync("/api/subscriptions", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions();
                var subscriptionResponse = JsonSerializer.Deserialize<SubscriptionResponse>(responseContent, options);
                
                // Store the subscription ID in TempData or Session for use in the confirmation page
                HttpContext.Session.SetInt32("SubscriptionId", subscriptionResponse?.Id ?? 0);
                
                _logger.LogInformation("Subscription request submitted for {CompanyName} by {ContactPerson}", CompanyName, ContactPerson);
                
                // Redirect to a confirmation page
                return RedirectToPage("./Confirmation");
            }
            else
            {
                _logger.LogError("API call failed with status code: {StatusCode}", response.StatusCode);
                ModelState.AddModelError(string.Empty, "Failed to submit subscription request. Please try again.");
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting subscription request");
            ModelState.AddModelError(string.Empty, "An error occurred while submitting your request. Please try again.");
            return Page();
        }
    }
}



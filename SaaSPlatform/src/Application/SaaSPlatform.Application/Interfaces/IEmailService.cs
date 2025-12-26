namespace SaaSPlatform.Application.Interfaces;

public interface IEmailService
{
    Task SendDeploymentStartedEmailAsync(string toEmail, string companyName, int subscriptionId);
    Task SendDeploymentCompletedEmailAsync(string toEmail, string companyName, string deploymentUrl);
    Task SendDeploymentFailedEmailAsync(string toEmail, string companyName, string errorMessage);
    Task SendWelcomeEmailAsync(string toEmail, string firstName, string lastName);
    Task SendSubscriptionCreatedEmailAsync(string toEmail, string companyName, string tier);
}

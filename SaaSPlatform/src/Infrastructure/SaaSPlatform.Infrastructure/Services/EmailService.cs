using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SaaSPlatform.Application.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SaaSPlatform.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ISendGridClient? _sendGridClient;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly bool _useSimulation;

    public EmailService(
        ILogger<EmailService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _fromEmail = configuration["SendGrid:FromEmail"] ?? "noreply@saasplatform.com";
        _fromName = configuration["SendGrid:FromName"] ?? "SaaS Platform";
        _useSimulation = configuration.GetValue<bool>("SendGrid:UseSimulation", true);

        var apiKey = configuration["SendGrid:ApiKey"];
        if (!_useSimulation && !string.IsNullOrEmpty(apiKey))
        {
            _sendGridClient = new SendGridClient(apiKey);
            _logger.LogInformation("SendGrid client initialized");
        }
        else
        {
            _useSimulation = true;
            _logger.LogInformation("Email service running in simulation mode");
        }
    }

    public async Task SendDeploymentStartedEmailAsync(string toEmail, string companyName, int subscriptionId)
    {
        var subject = $"Deployment Started - {companyName}";
        var htmlContent = $@"
            <h2>Deployment Started</h2>
            <p>Hello,</p>
            <p>We're pleased to inform you that the deployment for <strong>{companyName}</strong> has started.</p>
            <p>Subscription ID: {subscriptionId}</p>
            <p>You will receive another email once the deployment is complete.</p>
            <br/>
            <p>Best regards,<br/>SaaS Platform Team</p>";

        await SendEmailAsync(toEmail, subject, htmlContent);
    }

    public async Task SendDeploymentCompletedEmailAsync(string toEmail, string companyName, string deploymentUrl)
    {
        var subject = $"Deployment Completed - {companyName}";
        var htmlContent = $@"
            <h2>Deployment Completed Successfully!</h2>
            <p>Hello,</p>
            <p>Great news! The deployment for <strong>{companyName}</strong> has completed successfully.</p>
            <p>Your application is now available at: <a href='{deploymentUrl}'>{deploymentUrl}</a></p>
            <br/>
            <p>Best regards,<br/>SaaS Platform Team</p>";

        await SendEmailAsync(toEmail, subject, htmlContent);
    }

    public async Task SendDeploymentFailedEmailAsync(string toEmail, string companyName, string errorMessage)
    {
        var subject = $"Deployment Failed - {companyName}";
        var htmlContent = $@"
            <h2>Deployment Failed</h2>
            <p>Hello,</p>
            <p>Unfortunately, the deployment for <strong>{companyName}</strong> has failed.</p>
            <p>Error: {errorMessage}</p>
            <p>Our team has been notified and will investigate the issue. Please contact support if you need immediate assistance.</p>
            <br/>
            <p>Best regards,<br/>SaaS Platform Team</p>";

        await SendEmailAsync(toEmail, subject, htmlContent);
    }

    public async Task SendWelcomeEmailAsync(string toEmail, string firstName, string lastName)
    {
        var subject = "Welcome to SaaS Platform!";
        var htmlContent = $@"
            <h2>Welcome to SaaS Platform!</h2>
            <p>Hello {firstName} {lastName},</p>
            <p>Thank you for registering with SaaS Platform. We're excited to have you on board!</p>
            <p>You can now:</p>
            <ul>
                <li>Create and manage subscriptions</li>
                <li>Deploy your applications to Azure</li>
                <li>Monitor your resources in real-time</li>
            </ul>
            <p>If you have any questions, don't hesitate to reach out to our support team.</p>
            <br/>
            <p>Best regards,<br/>SaaS Platform Team</p>";

        await SendEmailAsync(toEmail, subject, htmlContent);
    }

    public async Task SendSubscriptionCreatedEmailAsync(string toEmail, string companyName, string tier)
    {
        var subject = $"Subscription Created - {companyName}";
        var htmlContent = $@"
            <h2>Subscription Created</h2>
            <p>Hello,</p>
            <p>A new subscription has been created:</p>
            <ul>
                <li><strong>Company:</strong> {companyName}</li>
                <li><strong>Tier:</strong> {tier}</li>
            </ul>
            <p>You can now deploy your subscription from the dashboard.</p>
            <br/>
            <p>Best regards,<br/>SaaS Platform Team</p>";

        await SendEmailAsync(toEmail, subject, htmlContent);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        if (_useSimulation)
        {
            _logger.LogInformation("SIMULATION: Would send email to {ToEmail} with subject '{Subject}'",
                toEmail, subject);
            return;
        }

        try
        {
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(toEmail);
            var plainTextContent = StripHtml(htmlContent);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await _sendGridClient!.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
            }
            else
            {
                var body = await response.Body.ReadAsStringAsync();
                _logger.LogError("Failed to send email to {ToEmail}. Status: {StatusCode}, Body: {Body}",
                    toEmail, response.StatusCode, body);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {ToEmail}", toEmail);
        }
    }

    private static string StripHtml(string html)
    {
        return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", " ")
            .Replace("  ", " ")
            .Trim();
    }
}

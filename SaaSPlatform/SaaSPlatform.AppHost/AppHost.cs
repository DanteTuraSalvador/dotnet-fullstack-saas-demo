using System.IO;

var builder = DistributedApplication.CreateBuilder(args);

// Add projects to Aspire orchestration
var api = builder.AddProject<Projects.SaaSPlatform_Api>("api");

var clientWeb = builder.AddProject<Projects.SaaSPlatform_Web_Client>("client-web")
    .WithReference(api);

var clientMvc = builder.AddProject<Projects.SaaSPlatform_Web_Client_Mvc>("client-mvc")
    .WithReference(api);

var adminWeb = builder.AddProject<Projects.SaaSPlatform_Web_Admin>("admin-web")
    .WithReference(api);

var adminMvc = builder.AddProject<Projects.SaaSPlatform_Web_Admin_Mvc>("admin-mvc")
    .WithReference(api);

var angularWorkingDirectory = Path.Combine("..", "src", "Presentation", "SaaSPlatform.Web.Client.Angular");

builder.AddExecutable("client-angular", "pwsh", "-NoLogo", "-File", "./run-angular.ps1", "-Port", "4300", "-ListenHost", "0.0.0.0")
    .WithWorkingDirectory(angularWorkingDirectory)
    .WithHttpEndpoint(port: 44300, targetPort: 4300)
    .WithReference(api);

var reactWorkingDirectory = Path.Combine("..", "src", "Presentation", "SaaSPlatform.Web.Client.React");

builder.AddExecutable("client-react", "pwsh", "-NoLogo", "-File", "./run-react.ps1", "-Port", "4400", "-ListenHost", "0.0.0.0")
    .WithWorkingDirectory(reactWorkingDirectory)
    .WithHttpEndpoint(port: 44400, targetPort: 4400)
    .WithReference(api);

var blazorWorkingDirectory = Path.Combine("..", "src", "Presentation", "SaaSPlatform.Web.Client.Blazor");

builder.AddExecutable("client-blazor", "pwsh", "-NoLogo", "-File", "./run-blazor.ps1",
        "-Port", "4500", "-ListenHost", "0.0.0.0", "-ApiBaseUrl", "https://localhost:7264")
    .WithWorkingDirectory(blazorWorkingDirectory)
    .WithHttpEndpoint(port: 44500, targetPort: 4500)
    .WithReference(api);

builder.Build().Run();

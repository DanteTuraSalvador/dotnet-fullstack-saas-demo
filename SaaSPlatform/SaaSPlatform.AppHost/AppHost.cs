var builder = DistributedApplication.CreateBuilder(args);

// Add projects to Aspire orchestration
var api = builder.AddProject<Projects.SaaSPlatform_Api>("api");

var clientWeb = builder.AddProject<Projects.SaaSPlatform_Web_Client>("client-web")
    .WithReference(api);

var adminWeb = builder.AddProject<Projects.SaaSPlatform_Web_Admin>("admin-web")
    .WithReference(api);

builder.Build().Run();

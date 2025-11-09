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

builder.Build().Run();

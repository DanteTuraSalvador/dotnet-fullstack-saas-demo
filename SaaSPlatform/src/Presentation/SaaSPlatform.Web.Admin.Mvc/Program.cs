using Microsoft.Extensions.Hosting;
using SaaSPlatform.Web.Admin.Mvc.Services;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<AdminSubscriptionService>();

var apiBaseAddress = builder.Configuration["services:api:0:uri"]
    ?? builder.Configuration["ApiClient:BaseAddress"]
    ?? "http://localhost:5153";

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

param(
    [string]$Port = "4500",
    [string]$ListenHost = "0.0.0.0",
    [string]$ApiBaseUrl = "https://localhost:7264"
)

$ErrorActionPreference = "Stop"

Write-Host "Starting Blazor dev server on $ListenHost`:$Port (API: $ApiBaseUrl)" -ForegroundColor Green
$env:ASPNETCORE_URLS = "http://$ListenHost`:$Port"
$env:ApiBaseUrl = $ApiBaseUrl
dotnet watch run --no-launch-profile --non-interactive

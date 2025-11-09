param(
    [string]$Port = "4300",
    [string]$ListenHost = "0.0.0.0"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path "./node_modules")) {
    Write-Host "node_modules not found. Running npm install..." -ForegroundColor Cyan
    npm install
}

Write-Host "Starting Angular dev server on $ListenHost`:$Port" -ForegroundColor Green
npm run start -- --port $Port --host $ListenHost

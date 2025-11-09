param(
    [string]$Port = "4400",
    [string]$ListenHost = "0.0.0.0"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path "./node_modules")) {
    Write-Host "node_modules not found. Running npm install..." -ForegroundColor Cyan
    npm install
}

Write-Host "Starting React dev server on $ListenHost`:$Port" -ForegroundColor Green
npm run dev -- --host $ListenHost --port $Port

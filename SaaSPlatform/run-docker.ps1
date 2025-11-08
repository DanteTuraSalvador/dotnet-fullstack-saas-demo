# SaaS Platform Docker Runner Script
# This script provides easy commands to run the SaaS platform with Docker

param(
    [Parameter(Position=0)]
    [string]$Command = "up"
)

Write-Host "=== SaaS Platform Docker Runner ==="
Write-Host ""

switch ($Command) {
    "up" {
        Write-Host "Starting all services..."
        docker-compose up -d
        Write-Host ""
        Write-Host "Services started successfully!"
        Write-Host "API Service: http://localhost:5153"
        Write-Host "Client Web: http://localhost:5000"
        Write-Host "Admin Web: http://localhost:5001"
        Write-Host "Seq Logging: http://localhost:5342"
        Write-Host ""
        Write-Host "To view logs: .\run-docker.ps1 logs"
        Write-Host "To stop services: .\run-docker.ps1 down"
    }
    "down" {
        Write-Host "Stopping all services..."
        docker-compose down
        Write-Host "Services stopped successfully!"
    }
    "logs" {
        Write-Host "Viewing service logs..."
        Write-Host "Press Ctrl+C to exit log view"
        docker-compose logs -f
    }
    "build" {
        Write-Host "Building all services..."
        docker-compose build
        Write-Host "Build completed!"
    }
    "status" {
        Write-Host "Checking service status..."
        docker-compose ps
    }
    default {
        Write-Host "Usage: .\run-docker.ps1 [up|down|logs|build|status]"
        Write-Host "  up     - Start all services (default)"
        Write-Host "  down   - Stop all services"
        Write-Host "  logs   - View service logs"
        Write-Host "  build  - Build all services"
        Write-Host "  status - Check service status"
    }
}
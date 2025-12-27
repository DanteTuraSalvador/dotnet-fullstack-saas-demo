# .NET Full-Stack SaaS Demo Platform

A comprehensive SaaS platform demonstrating full-stack .NET development capabilities with multiple frontend technologies, cloud automation, and enterprise practices.

## Project Overview

This project showcases expertise across the entire .NET ecosystem through a functional SaaS platform that automates Azure resource provisioning for clients. The platform serves as both a technical demonstration and a portfolio piece highlighting modern development practices.

## Features

### Core Platform
- **Multi-tenant SaaS Architecture** - Clean Architecture with Domain, Application, Infrastructure, and API layers
- **Client Subscription Management** - Full CRUD operations for managing client subscriptions
- **Azure Resource Provisioning** - Automated deployment of Azure resources (Resource Groups, App Service Plans, Web Apps, SQL Databases)
- **Background Job Processing** - Hangfire for asynchronous deployment tasks
- **Email Notifications** - SendGrid integration for deployment status notifications
- **Real-time Updates** - SignalR hub for live deployment progress

### Authentication & Security
- **JWT Bearer Authentication** - Token-based authentication for API access
- **ASP.NET Core Identity** - User management with role-based authorization
- **Role-Based Access Control** - Admin and Client roles with protected endpoints
- **Refresh Token Support** - Secure token refresh mechanism

### Observability & Operations
- **Health Checks** - SQL Server and self-health monitoring endpoints
- **Structured Logging** - Serilog with configurable sinks (Console, File, Seq)
- **Request Logging** - HTTP request/response logging for debugging

### Frontend Technologies
- **ASP.NET Core Razor Pages** - Server-rendered client and admin applications
- **ASP.NET Core MVC** - Traditional MVC client and admin variants
- **Angular 18 SPA** - Modern TypeScript SPA consuming the API
- **React 19 SPA** - Vite + TypeScript SPA with shared workflows
- **Blazor WebAssembly** - .NET-based SPA consuming the API
- **Tailwind CSS** - Consistent styling across all frontends

## Architecture

```
SaaSPlatform/
├── src/
│   ├── Core/
│   │   └── SaaSPlatform.Domain/              # Entities, Enums, Interfaces
│   ├── Application/
│   │   └── SaaSPlatform.Application/         # DTOs, Services, Business Logic
│   ├── Infrastructure/
│   │   └── SaaSPlatform.Infrastructure/      # DbContext, Repositories, External Services
│   ├── Api/
│   │   └── SaaSPlatform.Api/                 # Web API (REST endpoints)
│   └── Presentation/
│       ├── SaaSPlatform.Web.Client/          # Client Razor Pages
│       ├── SaaSPlatform.Web.Admin/           # Admin Razor Pages
│       ├── SaaSPlatform.Web.Client.Mvc/      # Client MVC
│       ├── SaaSPlatform.Web.Admin.Mvc/       # Admin MVC
│       ├── SaaSPlatform.Web.Client.Angular/  # Angular SPA
│       ├── SaaSPlatform.Web.Client.React/    # React SPA
│       └── SaaSPlatform.Web.Client.Blazor/   # Blazor WebAssembly
├── tests/
│   ├── SaaSPlatform.Domain.Tests/
│   ├── SaaSPlatform.Application.Tests/
│   ├── SaaSPlatform.Infrastructure.Tests/
│   └── SaaSPlatform.Api.Tests/
└── .aspire/
    ├── SaaSPlatform.AppHost/                 # Aspire orchestration
    └── SaaSPlatform.ServiceDefaults/         # Shared configuration
```

## Technology Stack

| Category | Technologies |
|----------|-------------|
| **Backend** | .NET 9.0, ASP.NET Core Web API, Entity Framework Core 9.0 |
| **Database** | SQL Server (LocalDB for development) |
| **Authentication** | ASP.NET Core Identity, JWT Bearer Tokens |
| **Background Jobs** | Hangfire with SQL Server storage |
| **Email** | SendGrid |
| **Real-time** | SignalR |
| **Cloud** | Azure SDK (Azure.ResourceManager) |
| **Logging** | Serilog |
| **Frontend** | Razor Pages, MVC, Angular 18, React 19, Blazor WASM |
| **Styling** | Tailwind CSS 3.4 |
| **Testing** | xUnit, Moq, FluentAssertions |
| **DevOps** | Docker, .NET Aspire, Azure DevOps Pipelines |

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server LocalDB (included with Visual Studio) or SQL Server Express
- Node.js 18+ (for Angular and React SPAs)

### Quick Start

```bash
# Clone the repository
git clone https://github.com/yourusername/dotnet-fullstack-saas-demo.git
cd dotnet-fullstack-saas-demo/SaaSPlatform

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update \
  --project src/Infrastructure/SaaSPlatform.Infrastructure \
  --startup-project src/Api/SaaSPlatform.Api

# Run the API
dotnet run --project src/Api/SaaSPlatform.Api
```

### Running with .NET Aspire (Recommended)

```bash
# Run with Aspire orchestration (starts all services)
dotnet run --project .aspire/SaaSPlatform.AppHost
```

### Running Individual SPAs

**Angular:**
```bash
cd src/Presentation/SaaSPlatform.Web.Client.Angular
npm install
npm start  # http://localhost:4200
```

**React:**
```bash
cd src/Presentation/SaaSPlatform.Web.Client.React
npm install
npm run dev  # http://localhost:5173
```

**Blazor:**
```bash
cd src/Presentation/SaaSPlatform.Web.Client.Blazor
dotnet run  # https://localhost:44500
```

## API Documentation

### Base URL
`https://localhost:7264/api`

### Authentication Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/auth/register` | Register a new user | No |
| POST | `/auth/login` | Login and get JWT token | No |
| POST | `/auth/refresh` | Refresh JWT token | No |
| POST | `/auth/revoke` | Revoke refresh token | Yes |
| GET | `/auth/me` | Get current user info | Yes |

#### Register Request
```json
POST /api/auth/register
{
  "email": "user@example.com",
  "password": "Password123",
  "firstName": "John",
  "lastName": "Doe"
}
```

#### Login Request
```json
POST /api/auth/login
{
  "email": "user@example.com",
  "password": "Password123"
}
```

#### Login Response
```json
{
  "success": true,
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "abc123...",
  "expiration": "2025-12-28T12:00:00Z"
}
```

### Subscription Endpoints

All subscription endpoints require authentication (Bearer token).

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/subscriptions` | Get all subscriptions |
| GET | `/subscriptions/{id}` | Get subscription by ID |
| POST | `/subscriptions` | Create new subscription |
| PUT | `/subscriptions/{id}/status` | Update subscription status |
| POST | `/subscriptions/{id}/deploy` | Deploy Azure resources |
| DELETE | `/subscriptions/{id}` | Delete subscription |

#### Create Subscription Request
```json
POST /api/subscriptions
{
  "companyName": "Acme Corp",
  "contactEmail": "contact@acme.com",
  "contactPerson": "Jane Smith",
  "businessType": "Technology"
}
```

#### Subscription Response
```json
{
  "id": 1,
  "companyName": "Acme Corp",
  "contactEmail": "contact@acme.com",
  "contactPerson": "Jane Smith",
  "businessType": "Technology",
  "createdDate": "2025-12-27T10:00:00Z",
  "subscriptionStatus": "Pending",
  "subscriptionTier": "Basic",
  "azureResourceGroup": null,
  "deploymentUrl": null
}
```

#### Update Status Request
```json
PUT /api/subscriptions/1/status
"Approved"
```

Valid status values: `Pending`, `Approved`, `Provisioning`, `Active`, `Failed`, `Suspended`, `Cancelled`

### Health Check Endpoints

| Endpoint | Description |
|----------|-------------|
| `/health` | Full health check (all components) |
| `/health/ready` | Readiness probe (database connectivity) |
| `/health/live` | Liveness probe (self-check) |

### SignalR Hub

Connect to `/hubs/deployment` for real-time deployment updates.

**Events:**
- `DeploymentProgress` - Receives deployment progress updates
- `DeploymentComplete` - Receives deployment completion notification

### Hangfire Dashboard

Access the Hangfire dashboard at `/hangfire` (requires Admin role in production).

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SaaSPlatformDb;Trusted_Connection=True;"
  },
  "Jwt": {
    "SecretKey": "your-secret-key-at-least-32-characters",
    "Issuer": "SaaSPlatform",
    "Audience": "SaaSPlatformClients",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  },
  "Azure": {
    "SubscriptionId": "your-azure-subscription-id",
    "TenantId": "your-tenant-id",
    "SimulationMode": true
  },
  "SendGrid": {
    "ApiKey": "your-sendgrid-api-key",
    "FromEmail": "noreply@yourplatform.com",
    "FromName": "SaaS Platform"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    },
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "File", "Args": { "path": "logs/log-.txt", "rollingInterval": "Day" } }
    ]
  }
}
```

## Testing

### Run All Tests

```bash
dotnet test
```

### Test Coverage

- **Domain Tests** - Entity validation and business rules
- **Application Tests** - Service layer and business logic
- **Infrastructure Tests** - Repository and external service integration
- **API Tests** - Controller unit tests

## Docker Support

### Run with Docker Compose

```bash
# Build and start all services
docker-compose up -d

# Using PowerShell script
.\run-docker.ps1 up      # Start all services
.\run-docker.ps1 down    # Stop all services
.\run-docker.ps1 logs    # View logs
.\run-docker.ps1 status  # Check service status
```

### Docker Services

| Service | Port | Description |
|---------|------|-------------|
| API | 5153 | REST API endpoints |
| Client Web | 5000 | Client-facing web app |
| Admin Web | 5001 | Admin dashboard |
| SQL Server | 1433 | Database |
| Seq | 5342 | Log aggregation |
| Azurite | 10000-10002 | Azure Storage emulator |

## Azure Deployment

The platform supports automated Azure resource provisioning:

1. **Resource Group** - Isolated container for client resources
2. **App Service Plan** - Hosting plan based on subscription tier
3. **Web App** - Client's web application
4. **SQL Server** - Database server
5. **SQL Database** - Client's database

### Subscription Tiers

| Tier | App Service SKU | SQL DTU |
|------|-----------------|---------|
| Basic | B1 | 5 |
| Standard | S1 | 10 |
| Premium | P1V2 | 50 |
| Enterprise | P2V2 | 100 |

## Development Workflow

### Branch Strategy

- `main` - Production-ready code
- `develop` - Integration branch
- `feature/*` - Feature development
- `sprint/*` - Sprint-specific work

### Commit Convention

```
feat: add new feature
fix: resolve bug
docs: update documentation
refactor: improve code structure
test: add or update tests
```

## License

MIT License - See LICENSE file for details.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run tests
5. Submit a pull request

# SaaS Platform Demo Walkthrough

This guide provides a step-by-step demonstration of the .NET Full-Stack SaaS Platform capabilities.

## Prerequisites

Before starting the demo, ensure you have:
- .NET 9.0 SDK installed
- SQL Server LocalDB running
- Node.js 18+ (for SPA demos)

## Quick Setup

```bash
cd SaaSPlatform

# Restore and build
dotnet restore
dotnet build

# Apply database migrations
dotnet ef database update \
  --project src/Infrastructure/SaaSPlatform.Infrastructure \
  --startup-project src/Api/SaaSPlatform.Api

# Run the API
dotnet run --project src/Api/SaaSPlatform.Api
```

The API will be available at `https://localhost:7264`

## Demo Scenarios

### 1. Authentication Flow

**Register a new user:**
```bash
curl -X POST https://localhost:7264/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "demo@example.com",
    "password": "Demo123!",
    "firstName": "Demo",
    "lastName": "User"
  }'
```

**Login and get JWT token:**
```bash
curl -X POST https://localhost:7264/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "demo@example.com",
    "password": "Demo123!"
  }'
```

Save the returned `token` for subsequent API calls.

### 2. Subscription Management

**Create a new subscription:**
```bash
curl -X POST https://localhost:7264/api/subscriptions \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <YOUR_TOKEN>" \
  -d '{
    "companyName": "Acme Corporation",
    "contactEmail": "admin@acme.com",
    "contactPerson": "John Smith",
    "businessType": "Technology"
  }'
```

**List all subscriptions:**
```bash
curl https://localhost:7264/api/subscriptions \
  -H "Authorization: Bearer <YOUR_TOKEN>"
```

**Update subscription status to Approved:**
```bash
curl -X PUT https://localhost:7264/api/subscriptions/1/status \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <YOUR_TOKEN>" \
  -d '"Approved"'
```

### 3. Azure Deployment (Simulation Mode)

**Trigger Azure resource deployment:**
```bash
curl -X POST https://localhost:7264/api/subscriptions/1/deploy \
  -H "Authorization: Bearer <YOUR_TOKEN>"
```

This creates:
- Resource Group
- App Service Plan
- Web App
- SQL Server & Database

In simulation mode, resources are not actually created in Azure but the workflow is demonstrated.

### 4. Health Checks

**Full health check:**
```bash
curl https://localhost:7264/health
```

**Readiness probe (database):**
```bash
curl https://localhost:7264/health/ready
```

**Liveness probe:**
```bash
curl https://localhost:7264/health/live
```

### 5. Real-time Updates (SignalR)

Connect to the SignalR hub at:
```
wss://localhost:7264/hubs/deployment
```

Listen for events:
- `DeploymentProgress` - Receives progress updates during deployment
- `DeploymentComplete` - Notification when deployment finishes

### 6. Hangfire Dashboard

Access the background jobs dashboard:
```
https://localhost:7264/hangfire
```

View:
- Active jobs
- Scheduled jobs
- Failed jobs
- Job history

## Frontend Demos

### Angular SPA

```bash
cd src/Presentation/SaaSPlatform.Web.Client.Angular
npm install
npm start
```

Open `http://localhost:4200`

Features to demonstrate:
- Subscription form with validation
- Real-time status updates
- Responsive design with Tailwind CSS

### React SPA

```bash
cd src/Presentation/SaaSPlatform.Web.Client.React
npm install
npm run dev
```

Open `http://localhost:5173`

Features to demonstrate:
- Modern React with hooks
- TypeScript integration
- Shared API consumption

### Blazor WebAssembly

```bash
cd src/Presentation/SaaSPlatform.Web.Client.Blazor
dotnet run
```

Open `https://localhost:44500`

Features to demonstrate:
- .NET in the browser
- Component-based architecture
- Shared API consumption

### Razor Pages (Server-rendered)

```bash
dotnet run --project src/Presentation/SaaSPlatform.Web.Client
```

Features to demonstrate:
- Server-side rendering
- Form handling
- Traditional web application pattern

## Architecture Highlights

### Clean Architecture Layers

1. **Domain** (`SaaSPlatform.Domain`)
   - Entities: `ClientSubscription`
   - Enums: `SubscriptionStatus`, `SubscriptionTier`
   - Pure business logic, no dependencies

2. **Application** (`SaaSPlatform.Application`)
   - DTOs for data transfer
   - Service interfaces
   - Business logic orchestration

3. **Infrastructure** (`SaaSPlatform.Infrastructure`)
   - Entity Framework Core DbContext
   - Repository implementations
   - External service integrations (Azure, SendGrid, Hangfire)

4. **API** (`SaaSPlatform.Api`)
   - RESTful controllers
   - JWT authentication
   - SignalR hubs
   - Middleware configuration

### Key Features Demo

| Feature | Location | Demo |
|---------|----------|------|
| JWT Auth | `AuthController.cs` | Login/Register API |
| CRUD Operations | `SubscriptionsController.cs` | Subscription management |
| Background Jobs | `DeploymentBackgroundJob.cs` | Async deployment |
| Real-time | `DeploymentHub.cs` | SignalR progress updates |
| Health Checks | `/health` endpoints | Kubernetes readiness |
| Logging | Serilog | Check `logs/` folder |

## Testing

Run all tests:
```bash
dotnet test
```

Test coverage:
- **Domain Tests**: Entity validation, enum parsing
- **Application Tests**: Service layer logic
- **Infrastructure Tests**: Repository, Azure service
- **API Tests**: Controller behavior

## Docker Demo

```bash
# Start all services
docker-compose up -d

# Check service status
docker-compose ps

# View logs
docker-compose logs -f api

# Stop services
docker-compose down
```

## Subscription Workflow

```
1. User submits subscription request
   └── Status: Pending

2. Admin reviews and approves
   └── Status: Approved

3. Admin triggers deployment
   └── Status: Provisioning
   └── Background job starts
   └── SignalR sends progress updates

4. Deployment completes
   └── Status: Active
   └── DeploymentUrl populated
   └── Email notification sent
```

## Key Talking Points

1. **Multiple Frontend Technologies** - Same API consumed by Angular, React, Blazor, Razor, and MVC

2. **Enterprise Patterns** - Clean Architecture, Repository Pattern, Dependency Injection

3. **Cloud-Ready** - Azure SDK integration, Health Checks, Structured Logging

4. **Real-time Capabilities** - SignalR for live updates during deployments

5. **Background Processing** - Hangfire for long-running tasks

6. **Security First** - JWT authentication, role-based authorization

7. **Full Test Coverage** - Unit and integration tests across all layers

8. **DevOps Ready** - Docker support, CI/CD pipeline, .NET Aspire orchestration

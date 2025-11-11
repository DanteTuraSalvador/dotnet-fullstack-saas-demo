# .NET Full-Stack SaaS Demo Platform

A comprehensive SaaS platform demonstrating full-stack .NET development capabilities with multiple frontend technologies, cloud automation, and enterprise practices.

## Development Plan - .NET Full-Stack SaaS Demo

### Project Timeline: 6 Sprints (11 weeks)

### Current Status: Sprint 2 - Frontend Modernization & Aspire Orchestration (In Progress)

**Goals:**
- [x] Create Aspire solution structure
- [x] Implement ClientWeb.Razor subscription form
- [x] Implement AdminWeb.Razor dashboard
- [x] Build Web API with CRUD operations
- [x] Set up SQL Server + Entity Framework
- [x] Integrate frontend applications with API
- [x] Create manual Azure deployment service
- [x] Basic Docker containerization

**Deliverables:**
- [x] Working subscription workflow
- [x] Database with ClientSubscription model
- [x] REST API endpoints
- [x] Frontend-to-API integration
- [x] Unified Tailwind UI across every frontend (Angular, React, Blazor, Razor, MVC, Admin portals)

## 🚀 Project Overview

This project showcases expertise across the entire .NET ecosystem through a functional SaaS platform that automates Azure resource provisioning for clients. The platform serves as both a technical demonstration and a portfolio piece highlighting modern development practices.

## 🗃️ Current Architecture

```
SaaSPlatform.sln
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
│       └── SaaSPlatform.Web.Admin/           # Admin Razor Pages
├── tests/
├── .aspire/
│   ├── SaaSPlatform.AppHost/                 # Aspire orchestration
│   └── SaaSPlatform.ServiceDefaults/         # Shared configuration
├── .gitignore
└── SaaSPlatform.sln
```

## 🏗️ Implemented Features (Sprint 1 - Phase A, B, C)

### ✅ Phase 1A: Project Setup & Solution Structure
- Professional .NET solution structure following Clean Architecture principles
- Proper project naming conventions with `SaaSPlatform.` prefix
- Aspire orchestration setup for cloud-native deployment

### ✅ Phase 1B: Database Models & DbContext
- `ClientSubscription` entity with comprehensive properties
- DTOs for data transfer between layers
- Entity Framework Core DbContext with proper configuration
- Database migrations and LocalDB setup

### ✅ Phase 1C: Web API Controllers
- RESTful API endpoints for subscription management
- Repository pattern implementation
- Service layer with business logic
- Full CRUD operations with proper HTTP status codes
- Dependency injection configuration

## 🛠️ Technology Stack

### Backend & Infrastructure
- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API backend
- **Entity Framework Core 9.0** - ORM with code-first migrations
- **SQL Server LocalDB** - Development database
- **.NET Aspire** - Application orchestration

### Frontend Technologies
- **ASP.NET Core Razor Pages** - Server-rendered applications
- **ASP.NET Core MVC** - Traditional MVC admin/client variants
- **Angular 18 SPA** - Modern client experience consuming shared API
- **React 19 SPA (Vite + TypeScript)** - Alternate SPA showcasing the same workflows
- **Blazor WebAssembly** - .NET-based SPA consuming the shared API
- **Clean Architecture** - Proper separation of concerns

### UI Styling & Components
- **Tailwind CSS 3.4** powers every frontend (Angular, React, Blazor, Razor, MVC, Admin) for a consistent system
- Custom utility classes (`.page-shell`, `.surface-card`, etc.) live in each app’s global stylesheet for shared spacing/typography
- Bootstrap assets have been removed; if you see stale files locally, clear `bin/obj` and re-run Aspire

### DevOps & Observability
- **.NET Aspire** - Application orchestration
- **Health Checks** - ASP.NET Core health monitoring

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- SQL Server LocalDB (included with Visual Studio) or SQL Server Express

### Quick Start
```bash
# Clone the repository (if applicable)
# git clone [repository-url]

# Navigate to the project directory
cd SaaSPlatform

# Restore .NET workloads
dotnet restore

# Install SPA dependencies (first run only)
cd src/Presentation/SaaSPlatform.Web.Client.Angular && npm install
cd ../SaaSPlatform.Web.Client.React && npm install
cd ../../../..  # back to the repo root

# Apply the latest migrations
dotnet ef database update \
  --project src/Infrastructure/SaaSPlatform.Infrastructure \
  --startup-project src/Api/SaaSPlatform.Api

# Launch the entire distributed app (Aspire orchestrates API + frontends)
dotnet run --project SaaSPlatform.AppHost/SaaSPlatform.AppHost.csproj
```

When the Aspire dashboard opens, use it to reach each frontend (Angular, React, Blazor, Razor, MVC, Admin).  
Aspire automatically runs the SPA dev servers via the included PowerShell scripts, so hot reload works out of the box.

### Angular Client (SPA)
```bash
cd src/Presentation/SaaSPlatform.Web.Client.Angular
npm install                      # first run
npm start                        # serves on http://localhost:4200
```
The Angular app calls the shared API at `https://localhost:7264` by default (see `src/environments/environment*.ts`).
Aspire auto-starts it via `run-angular.ps1`, but you can still run it manually for focused SPA work.

### React Client (SPA)
```bash
cd src/Presentation/SaaSPlatform.Web.Client.React
npm install          # first run
npm run dev          # serves on http://localhost:5173 by default
```
Set `VITE_API_BASE_URL` in `.env.*` if you expose the API on a different host/port. Under Aspire, the React client is accessible via the proxy URL (default https://localhost:44400).

### Blazor Client (SPA)
```bash
cd src/Presentation/SaaSPlatform.Web.Client.Blazor
dotnet run
```
The Blazor WebAssembly app reads `ApiBaseUrl` from `wwwroot/appsettings*.json` (defaults to `https://localhost:7264`). When hosted via Aspire, use the portal link (default https://localhost:44500) to open the dev server.

### Using .NET Aspire (Recommended)
```bash
# Run with .NET Aspire orchestration
dotnet run --project SaaSPlatform.AppHost
```

## 📁 Project Structure Details

### Core Layer (Domain)
- **SaaSPlatform.Domain**: Contains entities, enums, and interfaces that represent the business domain
- `Entities/ClientSubscription.cs`: Main entity for client subscription requests
- `SubscriptionStatus` enum: Status tracking for subscription lifecycle

### Application Layer
- **SaaSPlatform.Application**: Contains business logic, DTOs, and service interfaces
- `DTOs/`: Data Transfer Objects for API communication
- `Interfaces/`: Service and repository interfaces
- `Services/`: Implementation of business logic

### Infrastructure Layer
- **SaaSPlatform.Infrastructure**: Contains implementation details like DbContext and repositories
- `Data/AppDbContext.cs`: Entity Framework Core DbContext
- `Repositories/`: Implementation of repository interfaces
- `Migrations/`: Database migration files

### API Layer
- **SaaSPlatform.Api**: RESTful Web API endpoints
- `Controllers/SubscriptionsController.cs`: API endpoints for subscription management
- `Program.cs`: Application startup and dependency injection configuration

### Presentation Layer
- **SaaSPlatform.Web.Client**: Client-facing web application
- **SaaSPlatform.Web.Admin**: Admin dashboard for subscription management

### Orchestration
- **SaaSPlatform.AppHost**: .NET Aspire orchestration for running all services
- **SaaSPlatform.ServiceDefaults**: Shared configuration and services

## 🎯 API Endpoints

### Client Subscriptions
- `GET /api/subscriptions` - Get all subscriptions
- `GET /api/subscriptions/{id}` - Get a specific subscription
- `POST /api/subscriptions` - Create a new subscription
- `PUT /api/subscriptions/{id}/status` - Update subscription status
- `DELETE /api/subscriptions/{id}` - Delete a subscription

## 🗓️ Development Plan Overview

### Sprint 1: Foundation & Core Platform (2 weeks)
**Goals:**
- [x] Create Aspire solution structure
- [x] Implement ClientWeb.Razor subscription form
- [x] Implement AdminWeb.Razor dashboard
- [x] Build Web API with CRUD operations
- [x] Set up SQL Server + Entity Framework
- [x] Integrate frontend applications with API
- [x] Create manual Azure deployment service
- [x] Basic Docker containerization
- [x] **SET UP TESTING INFRASTRUCTURE**

**Deliverables:**
- Working subscription workflow
- Database with ClientSubscription model
- REST API endpoints
- Azure CLI script generation
- Basic Bootstrap UI

### Sprint 2: Multiple Frontend Technologies (2 weeks)
**Goals:**
- [x] Create MVC versions of client/admin interfaces
- [x] Build Angular SPA client interface
- [x] Build React SPA client interface
- [x] Build Blazor SPA client interface
- [ ] Ensure all frontends use shared Web API
- [ ] Implement consistent styling

**Deliverables:**
- [x] 5 frontend technologies working
- [ ] Shared API consumption patterns
- [ ] Consistent user experience
- [ ] Technology comparison documentation

### Sprint 3: Authentication & Security (2 weeks)
**Goals:**
- [ ] Implement JWT Bearer token authentication
- [ ] Add Cookie authentication for server-rendered apps
- [ ] Create role-based authorization (Admin/Client)
- [ ] Build user registration/login pages
- [ ] Secure API endpoints and UI routes

**Deliverables:**
- Multiple auth strategies working
- Protected routes and endpoints
- User management system
- Security best practices implemented

### Sprint 4: DevOps & Containerization (2 weeks)
**Goals:**
- [ ] Complete Docker containerization
- [ ] Set up .NET Aspire orchestration
- [ ] Implement Serilog + Seq logging
- [ ] Add health check endpoints
- [ ] Create basic CI/CD pipeline

**Deliverables:**
- Full Docker Compose setup
- Structured logging with Seq
- Health monitoring
- GitHub Actions pipeline

### Sprint 5: Advanced Features & Polish (2 weeks)
**Goals:**
- [ ] Implement automated Azure deployment
- [ ] Add email notification system
- [ ] Create real-time updates with SignalR
- [ ] Build enhanced admin dashboard
- [ ] Improve responsive design

**Deliverables:**
- One-click Azure deployment
- Email notifications
- Real-time status updates
- Professional admin interface

### Sprint 6: Final Integration & Demo Prep (1 week)
**Goals:**
- [ ] Complete integration testing
- [ ] Prepare demo data and scenarios
- [ ] Create documentation
- [ ] Performance optimization
- [ ] Demo script preparation

**Deliverables:**
- Fully integrated platform
- Comprehensive documentation
- Demo walkthrough script
- Production-ready codebase

## 🎯 Success Metrics
- All frontend technologies functional
- Complete Azure automation workflow
- Multiple authentication strategies
- Production-ready DevOps setup
- Professional demo capability

## 🛠 Technology Decisions
- Database: SQL Server + EF Core 9
- Authentication: JWT + Cookies + Azure AD
- Frontends: Razor Pages, MVC, Angular, React
- Cloud: Azure SDK + Resource Manager
- DevOps: Docker + .NET Aspire
- Monitoring: Serilog + Seq + Prometheus + Grafana

## 🧪 Testing Infrastructure

### Test Projects Structure

We have implemented a comprehensive testing infrastructure with dedicated test projects for each layer of our application:

- **SaaSPlatform.Domain.Tests**: Unit tests for domain entities and business logic
- **SaaSPlatform.Application.Tests**: Unit tests for application services and business logic
- **SaaSPlatform.Infrastructure.Tests**: Unit and integration tests for data access and external services
- **SaaSPlatform.Api.Tests**: Unit and integration tests for API controllers and endpoints

### Testing Tools & Frameworks

- **xUnit**: Primary testing framework
- **Moq**: Mocking framework for isolating dependencies
- **FluentAssertions**: Fluent API for assertions to improve test readability

### Continuous Integration

We have set up GitHub Actions for continuous testing with automatic test execution on every push and pull request.

## 🤝 Development Workflow

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

## 🐳 Docker Containerization

We have implemented Docker containerization for all services in our SaaS platform:

### Docker Services
- **API Service** - REST API endpoints (port 5153)
- **Client Web** - Client-facing web application (port 5000)
- **Admin Web** - Administrator dashboard (port 5001)
- **SQL Server** - Database service (port 1433)
- **Azurite** - Azure Storage emulator (ports 10000-10002)
- **Seq** - Logging service (port 5342)

### Docker Files
- [docker-compose.yml](file:///c%3A/Users/Dante%20Salvador/dotnet-fullstack-saas-demo/SaaSPlatform/docker-compose.yml) - Orchestration file for all services
- [src/Api/SaaSPlatform.Api/Dockerfile](file:///c%3A/Users/Dante%20Salvador/dotnet-fullstack-saas-demo/SaaSPlatform/src/Api/SaaSPlatform.Api/Dockerfile) - API service Dockerfile
- [src/Presentation/SaaSPlatform.Web.Client/Dockerfile](file:///c%3A/Users/Dante%20Salvador/dotnet-fullstack-saas-demo/SaaSPlatform/src/Presentation/SaaSPlatform.Web.Client/Dockerfile) - Client web Dockerfile
- [src/Presentation/SaaSPlatform.Web.Admin/Dockerfile](file:///c%3A/Users/Dante%20Salvador/dotnet-fullstack-saas-demo/SaaSPlatform/src/Presentation/SaaSPlatform.Web.Admin/Dockerfile) - Admin web Dockerfile
- [run-docker.ps1](file:///c%3A/Users/Dante%20Salvador/dotnet-fullstack-saas-demo/SaaSPlatform/run-docker.ps1) - PowerShell script to easily manage Docker services

### Running with Docker
To run the entire platform with Docker:

```bash
# Build and start all services
docker-compose up -d

# Or use our PowerShell script for easier management:
.\run-docker.ps1 up     # Start all services
.\run-docker.ps1 down   # Stop all services
.\run-docker.ps1 logs   # View logs
.\run-docker.ps1 build  # Build all services
.\run-docker.ps1 status # Check service status
```

This Docker setup provides a complete local development environment that simulates Azure services using containerized alternatives.

## ✅ Sprint 1 - Completed

All goals for Sprint 1 have been successfully completed! We have built a fully functional SaaS platform with:

1. **Clean Architecture** - Well-structured solution with separate layers
2. **Complete CRUD Operations** - Full API for managing client subscriptions
3. **Database Integration** - SQL Server with Entity Framework
4. **Frontend Applications** - Client and Admin web interfaces
5. **Testing Infrastructure** - Comprehensive test suite with 12 passing tests
6. **Azure Deployment Service** - Simulated Azure deployment workflow
7. **Docker Containerization** - Containerized services for easy deployment

The platform is ready for the next sprint which will focus on implementing multiple frontend technologies.

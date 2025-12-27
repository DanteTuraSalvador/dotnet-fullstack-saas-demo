# .NET Full-Stack SaaS Demo Platform

A comprehensive SaaS platform demonstrating full-stack .NET development capabilities with multiple frontend technologies, cloud automation, and enterprise practices.

## 🚀 Project Overview

This project showcases my expertise across the entire .NET ecosystem through a functional SaaS platform that automates Azure resource provisioning for clients. The platform serves as both a technical demonstration and a portfolio piece highlighting modern development practices.

### **Key Demonstrations**
- ✅ **Multiple Frontends**: Razor Pages, MVC, Angular, React
- ✅ **Backend API**: RESTful Web API with clean architecture
- ✅ **Authentication**: JWT, Cookies, Azure AD strategies
- ✅ **Cloud Integration**: Azure SDK & Resource Manager automation
- ✅ **DevOps**: Docker, .NET Aspire, CI/CD pipelines
- ✅ **Observability**: Serilog, Seq, Prometheus, Grafana
- ✅ **Enterprise Patterns**: Clean Architecture, Domain-Driven Design

## 📋 Business Context

A platform where:
- **Clients** request Azure infrastructure through multiple UI options
- **Admins** approve requests and automate deployments
- **System** provisions cloud resources automatically
- **All interactions** are logged, monitored, and secured

## 🗃️ Architecture

```
SaaSPlatform.sln
├── Presentation/
│   ├── ClientWeb.Razor (Razor Pages client)
│   ├── ClientWeb.Mvc (MVC client)
│   ├── ClientWeb.Angular (Angular SPA)
│   ├── ClientWeb.React (React SPA)
│   ├── AdminWeb.Razor (Razor Pages admin)
│   └── AdminWeb.Mvc (MVC admin)
├── WebApi/ (REST API backend)
├── Infrastructure/ (Azure services, email, external APIs)
├── Models/ (Entities, DTOs, ViewModels)
├── AppHost/ (.NET Aspire orchestration)
└── ServiceDefaults/ (Shared configuration, logging, health checks)
```

## 🗓️ Development Plan

### **Sprint Progress**

| Sprint | Status | Focus Area | Key Deliverables |
|--------|--------|------------|------------------|
| 1 | ✅ **Complete** | Foundation & Core Platform | Razor Pages + Web API + SQL Server |
| 2 | ✅ **Complete** | Multiple Frontend Technologies | MVC, Angular, React, Blazor implementations |
| 3 | ✅ **Complete** | Authentication & Security | JWT, ASP.NET Core Identity, Role-based auth |
| 4 | ✅ **Complete** | DevOps & Containerization | Docker, .NET Aspire, CI/CD Pipeline |
| 5 | ✅ **Complete** | Advanced Features | Health Checks, Serilog, SignalR real-time updates |
| 6 | ✅ **Complete** | Final Polish | Azure SDK, Hangfire, Email, Documentation |

## 🛠️ Technology Stack

### **Frontend Technologies**
- **ASP.NET Core Razor Pages** - Server-rendered applications
- **ASP.NET Core MVC** - Model-View-Controller pattern
- **Angular** - TypeScript-based SPA framework
- **React** - Component-based UI library with hooks

### **Backend & Infrastructure**
- **ASP.NET Core Web API 8** - RESTful API backend
- **Entity Framework Core 8** - ORM with code-first migrations
- **SQL Server** - Primary database with LocalDB for development
- **Azure SDK** - Programmatic Azure resource management

### **Security & Authentication**
- **JWT Bearer Tokens** - Stateless authentication for SPAs
- **Cookie Authentication** - Traditional server-rendered apps
- **Azure AD Integration** - Enterprise identity provider
- **Role-based Authorization** - Admin, Client roles with policies

### **DevOps & Observability**
- **Docker** - Containerization for all services
- **.NET Aspire** - Application orchestration
- **Serilog** - Structured logging with Seq sink
- **Prometheus + Grafana** - Metrics collection and visualization
- **Health Checks** - ASP.NET Core health monitoring

### **Cloud Services**
- **Azure App Service** - Web application hosting
- **Azure SQL Database** - Cloud database
- **Azure Resource Manager** - Infrastructure as code
- **Azure Storage** - Blob storage and static content

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or use Docker)
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli) (for deployment testing)
- [Node.js](https://nodejs.org/) (for Angular/React development)

### Quick Start

```bash
# Clone the repository
git clone https://github.com/yourusername/dotnet-fullstack-saas-demo.git
cd dotnet-fullstack-saas-demo

# Run with .NET Aspire (recommended)
dotnet run --project AppHost

# Or run with Docker Compose
docker-compose up -d
```

### Manual Setup

```bash
# Restore dependencies
dotnet restore

# Run database migrations
cd WebApi
dotnet ef database update

# Run individual services
dotnet run --project WebApi
dotnet run --project ClientWeb.Razor
dotnet run --project AdminWeb.Razor
```

## 📁 Project Structure

```
📦 dotnet-fullstack-saas-demo
├── 📂 Presentation
│   ├── 📂 ClientWeb.Razor          # Razor Pages client interface
│   ├── 📂 ClientWeb.Mvc            # MVC client interface
│   ├── 📂 ClientWeb.Angular        # Angular SPA client
│   ├── 📂 ClientWeb.React          # React SPA client
│   ├── 📂 AdminWeb.Razor           # Razor Pages admin dashboard
│   └── 📂 AdminWeb.Mvc             # MVC admin dashboard
├── 📂 WebApi                       # REST API backend
│   ├── 📂 Controllers
│   ├── 📂 Services
│   └── 📂 Models
├── 📂 Infrastructure               # External service integrations
│   ├── 📂 AzureServices
│   ├── 📂 EmailServices
│   └── 📂 DeploymentServices
├── 📂 Models                       # Shared data models
│   ├── 📂 Entities
│   ├── 📂 DTOs
│   └── 📂 ViewModels
├── 📂 AppHost                      # .NET Aspire orchestration
├── 📂 ServiceDefaults             # Shared configuration
├── 📜 docker-compose.yml          # Multi-container setup
├── 📜 DEVELOPMENT-PLAN.md         # Detailed development roadmap
└── 📜 README.md                   # This file
```

## 🎯 Demo Scenarios

### 1. Multi-Frontend Experience
- Same functionality across Razor Pages, MVC, Angular, and React
- Consistent API consumption patterns
- Technology comparison and appropriate use cases

### 2. Azure Automation Workflow
```
Client Request → Admin Approval → Automated Deployment → Resource Provisioning
     ↓               ↓               ↓               ↓
  Razor Form    MVC Dashboard    Azure SDK      Azure Resources
  Angular SPA                   CLI Commands    (App Service, SQL)
  React App
```

### 3. Security Showcase
- Multiple authentication strategies in one platform
- Role-based access control with Admin/Client roles
- Secure API communication with JWT validation

### 4. DevOps Practices
- Containerized development with Docker
- Structured logging with Seq interface
- Health monitoring and metrics collection
- CI/CD pipeline with GitHub Actions

## 🔧 Configuration

### Environment Variables

```bash
# Azure Configuration
AZURE_SUBSCRIPTION_ID=your_subscription_id
AZURE_TENANT_ID=your_tenant_id
AZURE_CLIENT_ID=your_client_id
AZURE_CLIENT_SECRET=your_client_secret

# Database Connection
ConnectionStrings__DefaultConnection=Server=localhost;Database=SaaSPlatform;Trusted_Connection=true;

# Application Settings
ASPNETCORE_ENVIRONMENT=Development
Serilog__WriteTo__0__Args__serverUrl=http://localhost:5341
```

### Docker Services

The platform runs these services in Docker:
- **SQL Server** - Database (port 1433)
- **Seq** - Log aggregation (port 5341)
- **Prometheus** - Metrics collection (port 9090)
- **Grafana** - Metrics visualization (port 3000)

## 📈 Monitoring & Logging

### Access Monitoring Interfaces
- **Seq Logs**: http://localhost:5341
- **Grafana Dashboards**: http://localhost:3000 (admin/admin)
- **Prometheus Metrics**: http://localhost:9090
- **Health Checks**: http://localhost:8080/health

### Key Metrics Tracked
- Application performance and response times
- Azure resource deployment success rates
- User authentication and authorization events
- Database query performance
- Container resource utilization

## 🤝 Development Workflow

### Branch Strategy
- `main` - Production-ready code
- `develop` - Integration branch
- `feature/*` - Feature development
- `sprint/*` - Sprint-specific work

### Commit Convention
```
feat: add Angular client interface
fix: resolve database connection issue
docs: update API documentation
refactor: improve Azure service structure
test: add subscription service tests
```

## 🛠 Troubleshooting

### Common Issues

**Database Connection Issues**
```bash
# Ensure SQL Server is running
docker ps | grep sqlserver

# Run migrations
dotnet ef database update --project WebApi
```

**Docker Container Problems**
```bash
# Rebuild and restart containers
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```

**Azure Authentication**
```bash
# Login to Azure CLI
az login

# Verify subscription access
az account show
```

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙋‍♂️ About the Developer

This project serves as a comprehensive demonstration of full-stack .NET development capabilities, showcasing:

- **Enterprise Architecture** - Clean Architecture, DDD patterns
- **Modern Frontend Development** - Multiple framework proficiency
- **Cloud Integration** - Azure services and automation
- **DevOps Practices** - Containerization, CI/CD, monitoring
- **Security Implementation** - Multiple authentication strategies

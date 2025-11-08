# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the solution and restore dependencies
COPY *.sln .
COPY src/Application/SaaSPlatform.Application/*.csproj ./src/Application/SaaSPlatform.Application/
COPY src/Core/SaaSPlatform.Domain/*.csproj ./src/Core/SaaSPlatform.Domain/
COPY SaaSPlatform.ServiceDefaults/*.csproj ./SaaSPlatform.ServiceDefaults/
COPY src/Presentation/SaaSPlatform.Web.Admin/*.csproj ./src/Presentation/SaaSPlatform.Web.Admin/

# Restore dependencies
RUN dotnet restore ./src/Presentation/SaaSPlatform.Web.Admin/SaaSPlatform.Web.Admin.csproj

# Copy the rest of the source code
COPY src/Application/SaaSPlatform.Application/. ./src/Application/SaaSPlatform.Application/
COPY src/Core/SaaSPlatform.Domain/. ./src/Core/SaaSPlatform.Domain/
COPY SaaSPlatform.ServiceDefaults/. ./SaaSPlatform.ServiceDefaults/
COPY src/Presentation/SaaSPlatform.Web.Admin/. ./src/Presentation/SaaSPlatform.Web.Admin/

# Build the Admin Web project
WORKDIR /app/src/Presentation/SaaSPlatform.Web.Admin
RUN dotnet publish -c Release -o out

# Use the official .NET 9.0 runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/src/Presentation/SaaSPlatform.Web.Admin/out .

# Expose port 8080
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "SaaSPlatform.Web.Admin.dll"]
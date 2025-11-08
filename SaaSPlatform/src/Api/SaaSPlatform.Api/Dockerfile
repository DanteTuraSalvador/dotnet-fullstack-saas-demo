# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the solution and restore dependencies
COPY *.sln .
COPY src/Core/SaaSPlatform.Domain/*.csproj ./src/Core/SaaSPlatform.Domain/
COPY src/Application/SaaSPlatform.Application/*.csproj ./src/Application/SaaSPlatform.Application/
COPY src/Infrastructure/SaaSPlatform.Infrastructure/*.csproj ./src/Infrastructure/SaaSPlatform.Infrastructure/
COPY src/Api/SaaSPlatform.Api/*.csproj ./src/Api/SaaSPlatform.Api/
COPY SaaSPlatform.ServiceDefaults/*.csproj ./SaaSPlatform.ServiceDefaults/

# Restore dependencies
RUN dotnet restore ./src/Api/SaaSPlatform.Api/SaaSPlatform.Api.csproj

# Copy the rest of the source code
COPY src/Core/SaaSPlatform.Domain/. ./src/Core/SaaSPlatform.Domain/
COPY src/Application/SaaSPlatform.Application/. ./src/Application/SaaSPlatform.Application/
COPY src/Infrastructure/SaaSPlatform.Infrastructure/. ./src/Infrastructure/SaaSPlatform.Infrastructure/
COPY src/Api/SaaSPlatform.Api/. ./src/Api/SaaSPlatform.Api/
COPY SaaSPlatform.ServiceDefaults/. ./SaaSPlatform.ServiceDefaults/

# Build the API project
WORKDIR /app/src/Api/SaaSPlatform.Api
RUN dotnet publish -c Release -o out

# Use the official .NET 9.0 runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/src/Api/SaaSPlatform.Api/out .

# Expose port 8080
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "SaaSPlatform.Api.dll"]
# Use .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

# Copy solution and project files
COPY *.sln ./
COPY BudgetTracker/*.csproj ./BudgetTracker/

# Restore dependencies
RUN dotnet restore

# Copy everything else and build
COPY . ./
WORKDIR /app/BudgetTracker
RUN dotnet publish -c Release -o /app/publish

# Use .NET 9.0 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# Railway will inject PORT environment variable at runtime
# Default to 8080 if PORT is not set
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BudgetTracker.dll"]

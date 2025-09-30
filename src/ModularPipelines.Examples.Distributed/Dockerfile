# Use .NET 9 SDK for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["ModularPipelines.sln", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["src/ModularPipelines/ModularPipelines.csproj", "src/ModularPipelines/"]
COPY ["src/ModularPipelines.Distributed/ModularPipelines.Distributed.csproj", "src/ModularPipelines.Distributed/"]
COPY ["src/ModularPipelines.DotNet/ModularPipelines.DotNet.csproj", "src/ModularPipelines.DotNet/"]
COPY ["src/ModularPipelines.Git/ModularPipelines.Git.csproj", "src/ModularPipelines.Git/"]
COPY ["src/ModularPipelines.Examples.Distributed/ModularPipelines.Examples.Distributed.csproj", "src/ModularPipelines.Examples.Distributed/"]

# Restore dependencies
RUN dotnet restore "src/ModularPipelines.Examples.Distributed/ModularPipelines.Examples.Distributed.csproj"

# Copy all source code
COPY ["src/", "src/"]

# Build the application
WORKDIR "/src/src/ModularPipelines.Examples.Distributed"
RUN dotnet build "ModularPipelines.Examples.Distributed.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ModularPipelines.Examples.Distributed.csproj" -c Release -o /app/publish

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Default command (override with docker-compose)
ENTRYPOINT ["dotnet", "ModularPipelines.Examples.Distributed.dll"]

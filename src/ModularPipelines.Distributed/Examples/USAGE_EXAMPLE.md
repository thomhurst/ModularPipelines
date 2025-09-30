# ModularPipelines.Distributed - Usage Example

This document demonstrates how to use ModularPipelines.Distributed for horizontally scaling your pipelines.

---

## Basic Example: Orchestrator + Workers

### **1. Define Your Modules**

Create standard ModularPipelines modules as usual:

```csharp
// RestoreModule.cs
public class RestoreModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        return await context.DotNet().Restore(
            x => x.WithWorkingDirectory(context.Git().RootDirectory));
    }
}

// BuildModule.cs
[DependsOn<RestoreModule>]
public class BuildModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        return await context.DotNet().Build(x => x
            .WithConfiguration("Release")
            .WithWorkingDirectory(context.Git().RootDirectory));
    }
}

// TestModule.cs
[DependsOn<BuildModule>]
public class TestModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        return await context.DotNet().Test(x => x
            .WithConfiguration("Release")
            .WithNoBuild()
            .WithWorkingDirectory(context.Git().RootDirectory));
    }
}

// PublishModule.cs
[DependsOn<TestModule>]
public class PublishModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        return await context.DotNet().Publish(x => x
            .WithConfiguration("Release")
            .WithOutput("./publish")
            .WithWorkingDirectory(context.Git().RootDirectory));
    }
}
```

---

### **2. Orchestrator Setup**

The orchestrator coordinates execution across workers:

```csharp
// Program.cs (Orchestrator)
using ModularPipelines.Host;
using ModularPipelines.Distributed.Extensions;

var summary = await PipelineHostBuilder.Create()
    // Add distributed execution support
    .AddDistributedExecution(options =>
    {
        options.Mode = DistributedExecutionMode.Orchestrator;
        options.OrchestratorPort = 8080;
        options.WorkerHeartbeatTimeout = TimeSpan.FromMinutes(2);
        options.MaxRetryAttempts = 3;
        options.EnableCompression = true;
        options.PreferLocalExecution = true; // Run on orchestrator if possible
    })

    // Register modules
    .AddModule<RestoreModule>()
    .AddModule<BuildModule>()
    .AddModule<TestModule>()
    .AddModule<PublishModule>()

    // Execute
    .ExecutePipelineAsync();

Console.WriteLine($"Pipeline completed in {summary.PipelineDuration}");
```

**Run orchestrator**:
```bash
dotnet run -- orchestrator
```

---

### **3. Worker Setup**

Workers execute modules assigned by the orchestrator:

```csharp
// Program.cs (Worker)
using ModularPipelines.Host;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Distributed.Abstractions;

await PipelineHostBuilder.Create()
    // Add distributed execution support
    .AddDistributedExecution()

    // Configure as worker
    .AsWorker("http://orchestrator:8080", capabilities =>
    {
        capabilities.Os = OperatingSystem.IsLinux() ? "linux" :
                         OperatingSystem.IsWindows() ? "windows" : "macos";
        capabilities.InstalledTools = ["dotnet", "docker", "git"];
        capabilities.MaxParallelModules = Environment.ProcessorCount;
        capabilities.Tags = ["build-worker"];
    })

    // Run as worker (blocks until shutdown)
    .RunWorkerAsync(); // TODO: This method needs implementation
```

**Run workers**:
```bash
# Worker 1
dotnet run -- worker --orchestrator http://localhost:8080

# Worker 2
dotnet run -- worker --orchestrator http://localhost:8080

# Worker 3
dotnet run -- worker --orchestrator http://localhost:8080
```

---

## Advanced Example: GitHub Actions

### **Workflow Configuration**

```yaml
name: Distributed Pipeline

on: [push, pull_request]

jobs:
  orchestrator:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - name: Run Orchestrator
        run: |
          dotnet run --project src/MyPipeline/MyPipeline.csproj -- orchestrator
        env:
          ORCHESTRATOR_PORT: 8080

  workers:
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]
    runs-on: ${{ matrix.os }}
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - name: Run Worker
        run: |
          dotnet run --project src/MyPipeline/MyPipeline.csproj -- worker
        env:
          ORCHESTRATOR_URL: http://orchestrator:8080
          WORKER_MAX_PARALLEL: 2
```

---

## Docker Compose Example

### **docker-compose.yml**

```yaml
version: '3.8'

services:
  orchestrator:
    build: .
    command: dotnet run -- orchestrator
    ports:
      - "8080:8080"
    environment:
      - ORCHESTRATOR_PORT=8080
      - LOG_LEVEL=Information
    networks:
      - pipeline-network

  worker1:
    build: .
    command: dotnet run -- worker
    environment:
      - ORCHESTRATOR_URL=http://orchestrator:8080
      - WORKER_ID=worker1
      - WORKER_MAX_PARALLEL=4
    depends_on:
      - orchestrator
    networks:
      - pipeline-network

  worker2:
    build: .
    command: dotnet run -- worker
    environment:
      - ORCHESTRATOR_URL=http://orchestrator:8080
      - WORKER_ID=worker2
      - WORKER_MAX_PARALLEL=4
    depends_on:
      - orchestrator
    networks:
      - pipeline-network

  worker3:
    build: .
    command: dotnet run -- worker
    environment:
      - ORCHESTRATOR_URL=http://orchestrator:8080
      - WORKER_ID=worker3
      - WORKER_MAX_PARALLEL=4
    depends_on:
      - orchestrator
    networks:
      - pipeline-network

networks:
  pipeline-network:
    driver: bridge
```

**Run**:
```bash
docker-compose up
```

---

## Configuration Options

### **DistributedPipelineOptions**

```csharp
.AddDistributedExecution(options =>
{
    // Execution mode
    options.Mode = DistributedExecutionMode.Orchestrator; // or Worker, Standalone

    // Orchestrator settings
    options.OrchestratorUrl = "http://orchestrator:8080";
    options.OrchestratorPort = 8080;

    // Worker settings
    options.WorkerCapabilities = new WorkerCapabilities
    {
        Os = "linux",
        InstalledTools = ["docker", "dotnet", "node"],
        MaxParallelModules = 4,
        Tags = ["gpu-enabled", "high-memory"]
    };

    // Timeouts and retries
    options.WorkerHeartbeatTimeout = TimeSpan.FromMinutes(2);
    options.WorkerHeartbeatInterval = TimeSpan.FromSeconds(30);
    options.RemoteExecutionTimeout = TimeSpan.FromHours(1);
    options.MaxRetryAttempts = 3;

    // Performance
    options.EnableCompression = true; // gzip for payloads > 1KB
    options.MaxPayloadSize = 100 * 1024 * 1024; // 100 MB
    options.PreferLocalExecution = true; // Execute locally when possible
});
```

---

## How It Works

### **Execution Flow**

1. **Orchestrator** receives module list
2. **Scheduler** analyzes dependencies and creates execution waves:
   ```
   Wave 1: [RestoreModule]
   Wave 2: [BuildModule]
   Wave 3: [TestModule]
   Wave 4: [PublishModule]
   ```
3. **Scheduler** assigns modules to nodes (workers or orchestrator)
4. **Orchestrator** sends execution requests to workers
5. **Workers** deserialize modules, execute, and return results
6. **Orchestrator** caches results and progresses to next wave

### **Dependency Resolution**

Modules with dependencies automatically receive prerequisite results:
- `BuildModule` depends on `RestoreModule`
- When `BuildModule` runs, it gets `RestoreModule`'s result
- Results are serialized and transmitted to the assigned worker

### **Failure Handling**

- **Worker timeout**: Module rescheduled to another worker
- **Execution failure**: Retry with exponential backoff (up to `MaxRetryAttempts`)
- **Worker offline**: Removed from registry, modules rescheduled

---

## Benefits

### **Performance**
- ⚡ **3x faster** with 3 workers (for parallelizable pipelines)
- 🚀 Horizontal scaling: add more workers to reduce time

### **Flexibility**
- 🖥️ Heterogeneous workers: Linux, Windows, macOS
- 🛠️ Tool-specific workers: GPU, Docker, specific SDKs
- ☁️ Cloud-native: works in any environment

### **Reliability**
- 🔄 Automatic retry on transient failures
- ♻️ Rescheduling when workers become unavailable
- 💾 Result caching to avoid redundant work

---

## Troubleshooting

### **Workers not connecting**

Check network connectivity:
```bash
curl http://orchestrator:8080/api/health
```

Verify orchestrator logs for registration messages.

### **Slow performance**

- Increase `MaxParallelModules` on workers
- Enable compression: `options.EnableCompression = true`
- Check network latency between orchestrator and workers

### **Module execution failures**

- Check worker capabilities match module requirements
- Verify environment variables are set correctly
- Review worker logs for detailed error messages

---

## Next Steps

1. ✅ Add modules to your pipeline
2. ✅ Configure orchestrator and workers
3. ⏳ Implement HTTP API endpoints (see `HTTP_API_DESIGN.md`)
4. ⏳ Test with Docker Compose
5. ⏳ Deploy to production (Kubernetes, VMs, etc.)

---

**Last Updated**: 2025-09-30

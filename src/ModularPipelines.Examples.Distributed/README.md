# ModularPipelines Distributed Execution Example

This example demonstrates how to use ModularPipelines.Distributed for horizontal scaling of pipeline workloads across multiple machines.

## Overview

The example includes a simple pipeline with 4 modules that demonstrate dependency relationships:

```
┌─────────────────────┐     ┌──────────────────────────┐
│ FetchDataModule     │     │ ValidateEnvironmentModule│
└──────────┬──────────┘     └─────────┬────────────────┘
           │                           │
           │                           │
           ▼                           │
    ┌──────────────────┐              │
    │ ProcessDataModule│              │
    └──────────┬───────┘              │
               │                      │
               ▼                      ▼
           ┌────────────────────────────┐
           │  PublishResultsModule      │
           └────────────────────────────┘
```

**Execution Waves:**
- Wave 1: `FetchDataModule` and `ValidateEnvironmentModule` run in parallel
- Wave 2: `ProcessDataModule` runs after `FetchDataModule` completes
- Wave 3: `PublishResultsModule` runs after both `ProcessDataModule` and `ValidateEnvironmentModule` complete

## Running Locally

### Option 1: Manual Setup (Multiple Terminals)

**Terminal 1 - Start Orchestrator:**
```bash
cd src/ModularPipelines.Examples.Distributed
dotnet run -- orchestrator 8080
```

**Terminal 2 - Start Worker 1:**
```bash
cd src/ModularPipelines.Examples.Distributed
dotnet run -- worker http://localhost:8080 worker1 9000
```

**Terminal 3 - Start Worker 2:**
```bash
cd src/ModularPipelines.Examples.Distributed
dotnet run -- worker http://localhost:8080 worker2 9001
```

### Option 2: Docker Compose (Recommended)

**Start the entire distributed system:**
```bash
cd src/ModularPipelines.Examples.Distributed
docker-compose up --build
```

This will start:
- 1 orchestrator on port 8080
- 3 workers (worker1, worker2, worker3)

**Stop the system:**
```bash
docker-compose down
```

## Command-Line Usage

### Orchestrator Mode
```bash
dotnet run -- orchestrator [port]

# Examples:
dotnet run -- orchestrator 8080
dotnet run -- orchestrator 5000
```

### Worker Mode
```bash
dotnet run -- worker [orchestrator-url] [worker-id] [worker-port]

# Examples:
dotnet run -- worker http://localhost:8080 worker1 9000
dotnet run -- worker http://192.168.1.100:8080 worker2 9001
dotnet run -- worker http://orchestrator:8080 my-worker 9000
```

**Default Values:**
- Orchestrator port: `8080`
- Orchestrator URL: `http://localhost:8080`
- Worker ID: auto-generated (e.g., `worker-HOSTNAME-abc123`)
- Worker port: `9000`

## Monitoring

### Check Orchestrator Health
```bash
curl http://localhost:8080/api/health
```

### List Available Workers
```bash
curl http://localhost:8080/api/workers
```

### Check Worker Health
```bash
curl http://localhost:9000/api/health
```

## Expected Output

### Orchestrator Output
```
ModularPipelines Distributed Execution Example
===============================================

🎯 Starting in ORCHESTRATOR mode

✓ Orchestrator HTTP API started on http://*:8080
⚙️ Starting module execution...

📥 Fetching data from external source...
🔍 Validating environment...
✓ Environment validation passed
✓ Data fetched successfully: Data fetched at 10:30:45
⚙️ Processing data: Data fetched at 10:30:45
✓ Data processed successfully
📤 Publishing results
✓ Results published successfully

📊 Pipeline Summary
   Status: Success
   Duration: 00:00:08
   Modules: 4
```

### Worker Output
```
ModularPipelines Distributed Execution Example
===============================================

⚙️ Starting in WORKER mode

   Orchestrator: http://localhost:8080
   Worker ID: worker1
   Worker Port: 9000

✓ Worker HTTP API started on http://*:9000
Registering worker with orchestrator...
✓ Worker successfully registered with orchestrator

Worker heartbeat service starting...
Executing module FetchDataModule (execution exec-123)
📥 Fetching data from external source...
✓ Data fetched successfully
✓ Module execution exec-123 completed successfully in 00:00:02
```

## How It Works

1. **Orchestrator starts** and listens on port 8080
2. **Workers start** and register with the orchestrator via HTTP POST to `/api/workers/register`
3. **Workers send heartbeats** every 30 seconds to `/api/workers/heartbeat`
4. **Pipeline begins execution**:
   - Orchestrator analyzes module dependencies
   - Creates execution waves (topologically sorted batches)
   - Assigns modules to available workers based on load and capabilities
5. **Module execution**:
   - Orchestrator sends HTTP POST to `/api/execution/execute` on selected worker
   - Worker deserializes module, executes it, and returns results
   - Orchestrator caches results and proceeds to next wave
6. **Pipeline completes** when all modules finish successfully

## Customization

### Add More Modules
Edit `Program.cs` and add your modules:
```csharp
.AddModule<YourCustomModule>()
```

### Adjust Worker Capabilities
Edit the worker configuration in `Program.cs`:
```csharp
.AsWorker(orchestratorUrl, capabilities =>
{
    capabilities.Os = "linux";
    capabilities.InstalledTools = new List<string> { "dotnet", "docker", "git" };
    capabilities.MaxParallelModules = 8;
    capabilities.Tags = new List<string> { "gpu-enabled", "high-memory" };
})
```

### Configure Retry and Timeout
Edit the orchestrator configuration in `Program.cs`:
```csharp
.AddDistributedExecution(options =>
{
    options.MaxRetryAttempts = 5;
    options.RemoteExecutionTimeout = TimeSpan.FromMinutes(30);
    options.WorkerHeartbeatTimeout = TimeSpan.FromMinutes(5);
})
```

## Troubleshooting

### Workers not connecting
- Ensure orchestrator is running and accessible
- Check firewall rules allow connections on ports 8080 (orchestrator) and 9000+ (workers)
- Verify orchestrator URL in worker command is correct
- Check orchestrator logs for registration messages

### Modules not executing
- Ensure all modules are registered on both orchestrator and workers using `.AddModule<T>()`
- Check worker capabilities match module requirements
- Review orchestrator logs for scheduling information

### Slow performance
- Increase worker count
- Adjust `MaxParallelModules` on workers
- Enable compression: `options.EnableCompression = true`
- Review module execution times and optimize bottlenecks

## Next Steps

- Explore `src/ModularPipelines.Distributed/` for implementation details
- Read `COMPLETED_IMPLEMENTATION.md` for architecture overview
- Check `HTTP_API_DESIGN.md` for API endpoint specifications
- Review `USAGE_EXAMPLE.md` for additional usage patterns

## Architecture

For complete architecture details, see:
- `../ModularPipelines.Distributed/README.md` - Overview
- `../ModularPipelines.Distributed/COMPLETED_IMPLEMENTATION.md` - Full implementation guide
- `../ModularPipelines.Distributed/HTTP_API_DESIGN.md` - API specifications

# ModularPipelines.Distributed

Distributed execution support for ModularPipelines enabling horizontal scaling across multiple machines.

## Current Implementation Status

### ✅ Completed Components

#### 1. **Core Abstractions** (`Abstractions/`)
- `INodeRegistry` - Interface for node discovery and registration
- `IExecutionNode` - Abstraction for local vs remote execution
- `IRemoteCommunicator` - Transport layer abstraction
- `IDistributedScheduler` - Scheduling interface
- `IResultCache` - Distributed result caching
- `WorkerNode` & `WorkerCapabilities` - Worker node models
- `DistributedExecutionPlan` - Execution planning model

#### 2. **Communication Protocol** (`Communication/Messages/`)
- `ModuleExecutionRequest` - Request for module execution
- `ModuleResultResponse` - Module execution result
- `WorkerRegistrationMessage` - Worker registration
- `HeartbeatMessage` - Health check messages
- `CancellationMessage` - Execution cancellation

#### 3. **Configuration** (`Options/`)
- `DistributedPipelineOptions` - Comprehensive configuration
- `DistributedExecutionMode` - Orchestrator/Worker/Standalone modes

### 🚧 Remaining Implementation

#### Phase 1: Core Functionality (Priority: HIGH)

1. **HTTP Communication** (`Communication/`)
   - `HttpRemoteCommunicator.cs` - Implement IRemoteCommunicator using HttpClient
   - Add Polly retry policies for resilience
   - Implement compression using gzip
   - Add timeout handling

2. **Node Registry** (`Registry/`)
   - `HttpNodeRegistry.cs` - In-memory registry with HTTP endpoints
   - Worker registration/unregistration
   - Heartbeat tracking and stale worker cleanup
   - Thread-safe concurrent access

3. **Execution Nodes** (`Engine/`)
   - `LocalExecutionNode.cs` - Execute modules locally on orchestrator
   - `RemoteExecutionNode.cs` - Delegate execution to workers
   - Result streaming and error handling

4. **Result Cache** (`Caching/`)
   - `MemoryResultCache.cs` - In-memory distributed cache
   - Thread-safe operations
   - Optional Redis implementation later

5. **Serialization** (`Serialization/`)
   - `ModuleSerializer.cs` - Serialize/deserialize modules
   - `ContextSerializer.cs` - Serialize minimal context for workers
   - Handle dependency result serialization

6. **Scheduling** (`Engine/`)
   - `DistributedScheduler.cs` - Basic round-robin scheduling
   - Consider worker capabilities and constraints
   - Handle NotInParallel and ParallelLimiter attributes
   - Load balancing algorithm

7. **Distributed Executor** (`Engine/`)
   - `DistributedModuleExecutor.cs` - Extends ModuleExecutor
   - Coordinate execution across nodes
   - Handle dependency transmission
   - Aggregate results from workers

#### Phase 2: Orchestrator & Worker Modes (Priority: HIGH)

8. **Orchestrator** (`Engine/`)
   - `DistributedOrchestrator.cs` - Main orchestrator logic
   - HTTP server for worker communication
   - Module assignment and tracking
   - Failure detection and rescheduling

9. **Worker** (`Engine/`)
   - `Worker.cs` - Worker node implementation
   - Module execution handler
   - Heartbeat sender
   - Graceful shutdown on drain signal

10. **Integration** (`Extensions/`)
    - `PipelineHostBuilderExtensions.cs` - AddDistributedExecution()
    - Service registration for DI
    - Configuration validation
    - Mode-specific setup

#### Phase 3: Advanced Features (Priority: MEDIUM)

11. **GitHub Actions Registry** (`Registry/`)
    - `GithubActionsNodeRegistry.cs` - Use GitHub Cache/Artifacts API
    - Worker discovery in CI/CD
    - Authentication and security

12. **Redis Backend** (`Caching/`, `Registry/`)
    - `RedisResultCache.cs` - Redis-backed result cache
    - `RedisNodeRegistry.cs` - Redis-backed registry
    - Production-ready scalability

13. **Advanced Scheduling**
    - Cost-based scheduling (minimize data transfer)
    - Affinity rules (prefer same worker for related modules)
    - Dynamic rescheduling on worker failure
    - Constraint satisfaction (NotInParallel across workers)

14. **Performance Optimizations**
    - gRPC support as alternative to HTTP
    - Result streaming for large payloads
    - Incremental result transmission
    - Connection pooling

#### Phase 4: Testing & Examples (Priority: HIGH)

15. **Unit Tests** (`test/ModularPipelines.Distributed.UnitTests/`)
    - Test all core abstractions
    - Mock HTTP communication
    - Test scheduling algorithms
    - Test failure scenarios

16. **Integration Tests**
    - End-to-end orchestrator-worker tests
    - Multi-worker scenarios
    - Failure recovery tests
    - Performance benchmarks

17. **Example Pipeline** (`examples/`)
    - Simple distributed pipeline example
    - GitHub Actions workflow example
    - Docker Compose setup for local testing
    - Documentation and README

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                       Orchestrator                          │
│  ┌───────────────┐  ┌───────────────┐  ┌───────────────┐  │
│  │ Scheduler     │  │ Node Registry │  │ Result Cache  │  │
│  └───────────────┘  └───────────────┘  └───────────────┘  │
│  ┌───────────────────────────────────────────────────────┐ │
│  │          Distributed Module Executor                  │ │
│  └───────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                             │
            HTTP/gRPC Communication
                             │
    ┌────────────────────────┴─────────────────────────┐
    │                      │                           │
┌───▼───────┐        ┌─────▼──────┐         ┌─────────▼──┐
│  Worker 1 │        │  Worker 2  │         │  Worker N  │
│           │        │            │         │            │
│ Executor  │        │  Executor  │         │  Executor  │
└───────────┘        └────────────┘         └────────────┘
```

## Usage Example

### Orchestrator Setup
```csharp
await PipelineHostBuilder.Create()
    .ConfigureServices((context, services) =>
    {
        services.Configure<DistributedPipelineOptions>(options =>
        {
            options.Mode = DistributedExecutionMode.Orchestrator;
            options.OrchestratorPort = 8080;
            options.WorkerHeartbeatTimeout = TimeSpan.FromMinutes(2);
        });
    })
    .AddDistributedExecution()
    .AddModule<BuildModule>()
    .AddModule<TestModule>()
    .AddModule<DeployModule>()
    .ExecuteAsync();
```

### Worker Setup
```csharp
await PipelineHostBuilder.Create()
    .ConfigureServices((context, services) =>
    {
        services.Configure<DistributedPipelineOptions>(options =>
        {
            options.Mode = DistributedExecutionMode.Worker;
            options.OrchestratorUrl = "http://orchestrator:8080";
            options.WorkerCapabilities = new WorkerCapabilities
            {
                Os = OS.Linux,
                InstalledTools = ["docker", "dotnet", "node"],
                MaxParallelModules = 4,
                Tags = ["gpu-enabled"]
            };
        });
    })
    .AddDistributedExecution()
    .RunWorkerAsync();
```

## Design Principles

1. **SOLID**: Each component has a single responsibility with well-defined interfaces
2. **DRY**: Reuse existing ModularPipelines infrastructure (serialization, execution, DI)
3. **KISS**: Start simple (HTTP+JSON), optimize later if needed
4. **Extensibility**: Plugin architecture for registries, communicators, caches
5. **Backward Compatibility**: Non-distributed pipelines work unchanged

## Next Steps

1. Implement `HttpRemoteCommunicator` (highest priority)
2. Implement `HttpNodeRegistry`
3. Implement `LocalExecutionNode` and `RemoteExecutionNode`
4. Implement `MemoryResultCache`
5. Implement `ModuleSerializer` and `ContextSerializer`
6. Implement `DistributedScheduler` with basic algorithm
7. Implement `DistributedModuleExecutor`
8. Implement orchestrator and worker modes
9. Add extension methods for PipelineHostBuilder
10. Create comprehensive tests
11. Create example pipeline with documentation

## Open Questions

1. **Shared State**: How to handle modules that modify shared state (file system, environment)?
   - Potential solution: Mark modules as requiring shared storage, mount volumes

2. **Pull vs Push**: Should workers pull modules or orchestrator push?
   - Current design: Orchestrator pushes (simpler, better control)
   - Future: Consider pull model for better scalability

3. **Cost Model**: How to optimize scheduling decisions?
   - Initial: Round-robin with capability matching
   - Future: Consider data transfer costs, execution history

4. **Security**: How to secure communication and prevent unauthorized workers?
   - Future: Add authentication tokens, TLS, worker verification

5. **Fault Tolerance**: How to handle partial failures?
   - Implement retry logic, checkpoint-based recovery

## Contributing

See implementation todos above. Each component should:
- Follow existing ModularPipelines patterns
- Include XML documentation
- Be thread-safe where applicable
- Support cancellation tokens
- Include proper error handling

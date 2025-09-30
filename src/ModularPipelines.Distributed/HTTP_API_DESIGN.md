# HTTP API Design for ModularPipelines.Distributed

This document describes the HTTP API endpoints needed for orchestrator-worker communication. These endpoints can be implemented using ASP.NET Core Minimal APIs, Carter, or any other HTTP framework.

---

## Orchestrator HTTP API

The orchestrator exposes these endpoints for workers to communicate with:

### **POST /api/workers/register**

Worker registration endpoint.

**Request Body**:
```json
{
  "workerNode": {
    "id": "worker-machine1-abc123",
    "endpoint": "http://worker1:9000",
    "capabilities": {
      "os": "linux",
      "installedTools": ["docker", "dotnet", "node"],
      "maxParallelModules": 4,
      "tags": ["gpu-enabled", "high-memory"]
    },
    "lastHeartbeat": "2025-09-30T10:00:00Z",
    "currentLoad": 0,
    "status": "Available"
  },
  "timestamp": "2025-09-30T10:00:00Z"
}
```

**Response (200 OK)**:
```json
{
  "success": true,
  "workerId": "worker-machine1-abc123",
  "heartbeatIntervalSeconds": 30
}
```

**Response (400 Bad Request)**:
```json
{
  "success": false,
  "errorMessage": "Worker capabilities are invalid"
}
```

**Implementation**:
```csharp
app.MapPost("/api/workers/register", async (
    WorkerRegistrationMessage message,
    INodeRegistry nodeRegistry) =>
{
    await nodeRegistry.RegisterWorkerAsync(message.WorkerNode);

    return Results.Ok(new WorkerRegistrationResponse
    {
        Success = true,
        WorkerId = message.WorkerNode.Id,
        HeartbeatIntervalSeconds = 30
    });
});
```

---

### **POST /api/workers/heartbeat**

Heartbeat update from worker.

**Request Body**:
```json
{
  "workerId": "worker-machine1-abc123",
  "currentLoad": 2,
  "timestamp": "2025-09-30T10:00:30Z"
}
```

**Response (200 OK)**:
```json
{
  "acknowledged": true,
  "shouldDrain": false
}
```

**Implementation**:
```csharp
app.MapPost("/api/workers/heartbeat", async (
    HeartbeatMessage message,
    INodeRegistry nodeRegistry) =>
{
    await nodeRegistry.UpdateHeartbeatAsync(message.WorkerId);

    return Results.Ok(new HeartbeatResponse
    {
        Acknowledged = true,
        ShouldDrain = false // Future: implement drain signals
    });
});
```

---

### **GET /api/workers**

List all available workers.

**Response (200 OK)**:
```json
[
  {
    "id": "worker-machine1-abc123",
    "endpoint": "http://worker1:9000",
    "capabilities": {
      "os": "linux",
      "installedTools": ["docker", "dotnet"],
      "maxParallelModules": 4,
      "tags": []
    },
    "lastHeartbeat": "2025-09-30T10:00:30Z",
    "currentLoad": 2,
    "status": "Busy"
  }
]
```

**Implementation**:
```csharp
app.MapGet("/api/workers", async (INodeRegistry nodeRegistry) =>
{
    var workers = await nodeRegistry.GetAvailableWorkersAsync();
    return Results.Ok(workers);
});
```

---

### **DELETE /api/workers/{workerId}**

Unregister a worker.

**Response (200 OK)**:
```json
{
  "success": true
}
```

**Implementation**:
```csharp
app.MapDelete("/api/workers/{workerId}", async (
    string workerId,
    INodeRegistry nodeRegistry) =>
{
    await nodeRegistry.UnregisterWorkerAsync(workerId);
    return Results.Ok(new { success = true });
});
```

---

### **GET /api/health**

Orchestrator health check.

**Response (200 OK)**:
```json
{
  "status": "healthy",
  "availableWorkers": 3,
  "timestamp": "2025-09-30T10:00:00Z"
}
```

---

## Worker HTTP API

Workers expose these endpoints for the orchestrator to communicate with:

### **POST /api/execution/execute**

Execute a module on this worker.

**Request Body**:
```json
{
  "executionId": "exec-123",
  "serializedModule": "{...}",
  "moduleTypeName": "MyApp.Modules.BuildModule, MyApp",
  "dependencyResults": {
    "MyApp.Modules.RestoreModule, MyApp": "{...}"
  },
  "environmentVariables": {
    "CI": "true",
    "BUILD_NUMBER": "42"
  },
  "workingDirectory": "/app/workspace",
  "timeout": "01:00:00",
  "timestamp": "2025-09-30T10:00:00Z"
}
```

**Response (200 OK)**:
```json
{
  "executionId": "exec-123",
  "success": true,
  "serializedResult": "{...}",
  "duration": "00:02:30",
  "startTime": "2025-09-30T10:00:00Z",
  "endTime": "2025-09-30T10:02:30Z",
  "workerId": "worker-machine1-abc123"
}
```

**Response (500 Internal Server Error)**:
```json
{
  "executionId": "exec-123",
  "success": false,
  "errorMessage": "Module execution failed",
  "exceptionType": "System.InvalidOperationException",
  "stackTrace": "at ...",
  "duration": "00:00:05",
  "startTime": "2025-09-30T10:00:00Z",
  "endTime": "2025-09-30T10:00:05Z",
  "workerId": "worker-machine1-abc123"
}
```

**Implementation**:
```csharp
app.MapPost("/api/execution/execute", async (
    ModuleExecutionRequest request,
    WorkerModuleExecutionHandler handler,
    IOptions<DistributedPipelineOptions> options) =>
{
    var workerId = GetWorkerId(options.Value);
    var response = await handler.ExecuteModuleAsync(request, workerId);

    return response.Success
        ? Results.Ok(response)
        : Results.StatusCode(500).WithBody(response);
});
```

---

### **POST /api/execution/cancel**

Cancel a module execution.

**Request Body**:
```json
{
  "executionId": "exec-123",
  "reason": "Timeout exceeded",
  "timestamp": "2025-09-30T10:05:00Z"
}
```

**Response (200 OK)**:
```json
{
  "success": true
}
```

**Response (404 Not Found)**:
```json
{
  "success": false,
  "errorMessage": "Execution not found"
}
```

**Implementation**:
```csharp
app.MapPost("/api/execution/cancel", (
    CancellationMessage message,
    WorkerModuleExecutionHandler handler) =>
{
    var cancelled = handler.CancelExecution(message.ExecutionId);

    return cancelled
        ? Results.Ok(new CancellationResponse { Success = true })
        : Results.NotFound(new CancellationResponse
          {
              Success = false,
              ErrorMessage = "Execution not found"
          });
});
```

---

### **GET /api/health**

Worker health check.

**Response (200 OK)**:
```json
{
  "status": "healthy",
  "currentLoad": 2,
  "maxLoad": 4,
  "timestamp": "2025-09-30T10:00:00Z"
}
```

**Implementation**:
```csharp
app.MapGet("/api/health", (
    WorkerModuleExecutionHandler handler,
    IOptions<DistributedPipelineOptions> options) =>
{
    var currentLoad = handler.GetCurrentExecutionCount();
    var maxLoad = options.Value.WorkerCapabilities?.MaxParallelModules ?? 1;

    return Results.Ok(new
    {
        status = "healthy",
        currentLoad,
        maxLoad,
        timestamp = DateTimeOffset.UtcNow
    });
});
```

---

## Implementation Guide

### **Option 1: ASP.NET Core Minimal APIs**

Add to your `csproj`:
```xml
<PackageReference Include="Microsoft.AspNetCore.App" />
```

Create a hosted service that starts Kestrel:
```csharp
public class OrchestratorApiService : IHostedService
{
    private WebApplication? _app;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var builder = WebApplication.CreateBuilder();

        // Configure services
        builder.Services.AddSingleton<INodeRegistry>(/* ... */);

        _app = builder.Build();

        // Map endpoints (see above)
        _app.MapPost("/api/workers/register", /* ... */);
        _app.MapPost("/api/workers/heartbeat", /* ... */);
        // ... etc

        await _app.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_app != null)
        {
            await _app.StopAsync(cancellationToken);
            await _app.DisposeAsync();
        }
    }
}
```

Register in `PipelineHostBuilderExtensions.cs`:
```csharp
services.AddHostedService<OrchestratorApiService>();
```

---

### **Option 2: Carter**

Carter provides a cleaner module-based approach:

```bash
dotnet add package Carter
```

Create modules:
```csharp
public class WorkerEndpointsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/workers/register", /* ... */);
        app.MapPost("/api/workers/heartbeat", /* ... */);
    }
}
```

---

### **Option 3: Custom HTTP Listener**

For minimal dependencies, use `HttpListener`:

```csharp
public class SimpleHttpApiService : IHostedService
{
    private readonly HttpListener _listener = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _listener.Prefixes.Add("http://+:8080/");
        _listener.Start();

        Task.Run(() => HandleRequestsAsync(cancellationToken), cancellationToken);

        return Task.CompletedTask;
    }

    private async Task HandleRequestsAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var context = await _listener.GetContextAsync();
            await ProcessRequestAsync(context);
        }
    }
}
```

---

## Security Considerations

For production deployments:

1. **Authentication**: Add bearer token or API key authentication
2. **HTTPS**: Use TLS for all communication
3. **Authorization**: Verify worker identity before accepting registrations
4. **Rate Limiting**: Prevent DOS attacks
5. **Input Validation**: Sanitize all inputs
6. **CORS**: Configure if accessing from web clients

---

## Testing the API

### Using `curl`:

```bash
# Register worker
curl -X POST http://localhost:8080/api/workers/register \
  -H "Content-Type: application/json" \
  -d @worker-registration.json

# Send heartbeat
curl -X POST http://localhost:8080/api/workers/heartbeat \
  -H "Content-Type: application/json" \
  -d '{"workerId":"worker-123","currentLoad":2}'

# Execute module
curl -X POST http://localhost:9000/api/execution/execute \
  -H "Content-Type: application/json" \
  -d @execution-request.json
```

### Using Postman/Insomnia:

Import the endpoint definitions from this document.

---

## Next Steps

1. Choose an HTTP framework (ASP.NET Core Minimal APIs recommended)
2. Implement orchestrator endpoints in `OrchestratorApiService.cs`
3. Implement worker endpoints in `WorkerApiService.cs`
4. Register services in `PipelineHostBuilderExtensions.cs`
5. Test with Docker Compose (orchestrator + 2 workers)
6. Add authentication and HTTPS for production

---

**Last Updated**: 2025-09-30

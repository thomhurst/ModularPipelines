# Quickstart: Distributed Workers Mode

**Date**: 2026-02-22 | **Branch**: `001-distributed-workers`

## Prerequisites

- Existing ModularPipelines project with registered modules
- `ModularPipelines.Distributed` NuGet package
- A coordination provider (custom implementation or future `ModularPipelines.Distributed.Redis` package)

## 1. Enable Distributed Mode

```csharp
// Program.cs
var builder = Pipeline.CreateBuilder(args);

// Register your modules as normal
builder.AddModule<BuildModule>();
builder.AddModule<TestModule>();
builder.AddModule<PublishModule>();

// Enable distributed mode
builder.AddDistributedMode(options =>
{
    options.InstanceIndex = int.Parse(args["--instance"]);
    options.TotalInstances = int.Parse(args["--total"]);
});

// Register your coordination provider
builder.AddDistributedCoordinator<MyRedisCoordinator>();

await builder.ExecutePipelineAsync();
```

## 2. Run with GitHub Actions Matrix

```yaml
jobs:
  pipeline:
    strategy:
      matrix:
        instance: [0, 1, 2, 3]
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'
      - run: |
          dotnet run --project src/MyPipeline -- \
            --instance=${{ matrix.instance }} \
            --total=4
        env:
          REDIS_URL: ${{ secrets.REDIS_URL }}
```

Instance 0 automatically becomes the master. Instances 1-3 become workers.

## 3. Pin Modules to Master

```csharp
[PinToMaster]
[DependsOn<TestModule>]
public class PublishToNuGetModule : Module<None>
{
    protected override async Task<None?> ExecuteAsync(
        IModuleContext context, CancellationToken ct)
    {
        // This only runs on the master instance
        await context.DotNet().NuGet.Push(...);
        return default;
    }
}
```

## 4. Require Worker Capabilities

```csharp
[RequiresCapability("docker")]
public class DockerBuildModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context, CancellationToken ct)
    {
        return await context.Docker().Build(...);
    }
}
```

Workers advertise capabilities in their configuration:

```csharp
builder.AddDistributedMode(options =>
{
    options.InstanceIndex = int.Parse(args["--instance"]);
    options.TotalInstances = int.Parse(args["--total"]);
    options.Capabilities.Add("docker");
    options.Capabilities.Add("high-memory");
    // OS capability (e.g., "linux") is auto-detected by default
});
```

## 5. Cross-Platform Testing with Matrix Modules

```csharp
[MatrixTarget("windows", "linux", "macos")]
public class CrossPlatformTests : Module<TestResult>
{
    protected override async Task<TestResult?> ExecuteAsync(
        IModuleContext context, CancellationToken ct)
    {
        var target = context.GetMatrixTarget(); // "windows", "linux", or "macos"
        // Run platform-specific tests
        return await RunTests(target);
    }
}
```

This registers as 3 modules: `CrossPlatformTests[windows]`, `CrossPlatformTests[linux]`, `CrossPlatformTests[macos]`. Each routes to a worker with the matching OS capability. In single-instance mode, only the local OS variant executes; others are skipped.

## 6. Implement a Custom Coordination Provider

```csharp
public class MyRedisCoordinator : IDistributedCoordinator
{
    private readonly IDatabase _db;

    public MyRedisCoordinator(IOptions<RedisOptions> options)
    {
        var redis = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
        _db = redis.GetDatabase();
    }

    public async Task EnqueueModuleAsync(
        ModuleAssignment assignment, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(assignment);
        await _db.ListRightPushAsync("work:queue", json);
    }

    public async Task<ModuleAssignment?> DequeueModuleAsync(
        IReadOnlySet<string> workerCapabilities, CancellationToken ct)
    {
        var json = await _db.ListLeftPopAsync("work:queue");
        if (json.IsNull) return null;
        var assignment = JsonSerializer.Deserialize<ModuleAssignment>(json!);
        if (!assignment.RequiredCapabilities.IsSubsetOf(workerCapabilities))
        {
            // Re-enqueue if this worker can't handle it
            await _db.ListRightPushAsync("work:queue", json);
            return null;
        }
        return assignment;
    }

    // ... implement remaining methods
}
```

## Without Distributed Mode

If you don't enable distributed mode, everything works exactly as before. No behavioral changes, no performance impact.

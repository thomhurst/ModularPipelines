---
title: "CI Example: GitHub Actions"
sidebar_position: 5
---

# CI Example: GitHub Actions

This is a complete example of running a distributed pipeline across GitHub Actions matrix runners using Redis for coordination.

## Workflow File

```yaml
name: Distributed Pipeline

on:
  push:
    branches: [main]
  pull_request:

jobs:
  pipeline:
    strategy:
      fail-fast: false
      matrix:
        instance: [0, 1, 2, 3]
        include:
          - instance: 0
            os: ubuntu-latest    # Master
          - instance: 1
            os: ubuntu-latest    # Linux worker
          - instance: 2
            os: windows-latest   # Windows worker
          - instance: 3
            os: macos-latest     # macOS worker

    runs-on: ${{ matrix.os }}

    steps:
      - uses: actions/checkout@v4

      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: "10.0.x"

      - name: Run Pipeline
        env:
          INSTANCE_INDEX: ${{ matrix.instance }}
          TOTAL_INSTANCES: 4
          REDIS_URL: ${{ secrets.REDIS_URL }}
        run: dotnet run --project src/MyPipeline -c Release
```

## Redis Setup for CI

You need a Redis instance that all runners can reach. Since GitHub Actions runners don't share a network, you need an externally hosted Redis.

### Option 1: Upstash (Recommended for CI)

[Upstash](https://upstash.com/) provides free serverless Redis with a REST API. The free tier includes 10,000 commands/day, which is sufficient for most pipeline runs.

1. Create a free Upstash database.
2. Copy the connection string (looks like `your-endpoint.upstash.io:6379,password=your-password,ssl=True`).
3. Add it as a GitHub Actions secret named `REDIS_URL`.

### Option 2: Redis Cloud

[Redis Cloud](https://redis.com/cloud/) offers a free 30MB plan. Create a database, note the public endpoint and password, and add the connection string as a secret.

### Option 3: Self-Hosted Redis

If you have a Redis instance accessible from the internet (or via a VPN), use its connection string directly.

## Pipeline Code

```csharp
using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Distributed.Redis.Extensions;
using ModularPipelines.Modules;
using Microsoft.Extensions.DependencyInjection;

var builder = Pipeline.CreateBuilder(args);

var instanceIndex = int.Parse(
    Environment.GetEnvironmentVariable("INSTANCE_INDEX") ?? "0");
var totalInstances = int.Parse(
    Environment.GetEnvironmentVariable("TOTAL_INSTANCES") ?? "1");

builder.AddDistributedMode(o =>
{
    o.InstanceIndex = instanceIndex;
    o.TotalInstances = totalInstances;
});

builder.AddRedisDistributedCoordinator(o =>
{
    o.ConnectionString = Environment.GetEnvironmentVariable("REDIS_URL")!;
    // RunIdentifier auto-detected from GITHUB_SHA
});

builder.Services.AddModule<RestoreModule>();
builder.Services.AddModule<LinuxBuildModule>();
builder.Services.AddModule<WindowsBuildModule>();
builder.Services.AddModule<MacBuildModule>();
builder.Services.AddModule<AggregateResultsModule>();

await builder.Build().RunAsync();

public class RestoreModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.DotNet().Restore(new());
        return "restored";
    }
}

[RequiresCapability("linux")]
[DependsOn<RestoreModule>]
public class LinuxBuildModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.DotNet().Build(new());
        await context.DotNet().Test(new());
        return "linux-ok";
    }
}

[RequiresCapability("windows")]
[DependsOn<RestoreModule>]
public class WindowsBuildModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.DotNet().Build(new());
        await context.DotNet().Test(new());
        return "windows-ok";
    }
}

[RequiresCapability("macos")]
[DependsOn<RestoreModule>]
public class MacBuildModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.DotNet().Build(new());
        await context.DotNet().Test(new());
        return "macos-ok";
    }
}

[PinToMaster]
[DependsOn<LinuxBuildModule>]
[DependsOn<WindowsBuildModule>]
[DependsOn<MacBuildModule>]
public class AggregateResultsModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        var linux = await context.GetModule<LinuxBuildModule>();
        var windows = await context.GetModule<WindowsBuildModule>();
        var mac = await context.GetModule<MacBuildModule>();

        return $"All platforms passed: {linux.ValueOrDefault}, "
             + $"{windows.ValueOrDefault}, {mac.ValueOrDefault}";
    }
}
```

## How It Works

1. GitHub Actions starts 4 matrix jobs in parallel (instances 0-3).
2. Instance 0 (master) builds the dependency graph and enqueues `RestoreModule`.
3. Any available worker picks up `RestoreModule`, executes it, and publishes the result.
4. The master sees the restore result and enqueues the three platform-specific build modules.
5. Each build module is routed to the worker with the matching OS capability.
6. All three builds run in parallel on different runners.
7. Once all builds complete, `AggregateResultsModule` runs on the master (pinned).
8. The master produces the pipeline summary and exits.

## Important Notes

- **Matrix jobs don't start simultaneously.** GitHub Actions may stagger runner provisioning. Workers that start before the master will wait for work to appear in the queue.
- **Runner timeout.** GitHub Actions has a 6-hour job timeout. Set `KeyExpirationSeconds` accordingly if you have very long pipelines.
- **fail-fast: false** is important — you don't want GitHub to cancel workers if the master reports an error in one module.
- **Secrets** — store your Redis connection string as a repository or organization secret, not in code.

## Azure DevOps

The same pattern works in Azure DevOps with a matrix strategy. The run identifier auto-detects from `BUILD_SOURCEVERSION`:

```yaml
strategy:
  matrix:
    master:
      INSTANCE_INDEX: 0
      vmImage: "ubuntu-latest"
    worker-linux:
      INSTANCE_INDEX: 1
      vmImage: "ubuntu-latest"
    worker-windows:
      INSTANCE_INDEX: 2
      vmImage: "windows-latest"
```

## GitLab CI

For GitLab CI, the run identifier auto-detects from `CI_COMMIT_SHA`. Use parallel jobs or a matrix to spawn instances:

```yaml
pipeline:
  parallel:
    matrix:
      - INSTANCE_INDEX: [0, 1, 2]
  script:
    - dotnet run --project src/MyPipeline -c Release
  variables:
    TOTAL_INSTANCES: 3
    REDIS_URL: $REDIS_URL
```

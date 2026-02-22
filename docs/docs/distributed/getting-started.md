---
title: Getting Started
sidebar_position: 2
---

# Getting Started with Distributed Mode

This guide walks you through adding distributed execution to an existing ModularPipelines project using Redis as the coordinator.

## Prerequisites

- A working ModularPipelines pipeline.
- A Redis instance accessible to all pipeline instances. Options include:
  - [Upstash](https://upstash.com/) — free serverless Redis, great for CI.
  - [Redis Cloud](https://redis.com/cloud/) — managed Redis with a free tier.
  - Local Redis via Docker: `docker run -d -p 6379:6379 redis`

## 1. Install the Package

Add the `ModularPipelines.Distributed.Redis` NuGet package to your pipeline project:

```bash
dotnet add package ModularPipelines.Distributed.Redis
```

This brings in `ModularPipelines.Distributed` (core distributed abstractions) and `StackExchange.Redis` automatically.

## 2. Configure the Pipeline

In your `Program.cs`, enable distributed mode and register the Redis coordinator:

```csharp
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Distributed.Redis.Extensions;

var builder = Pipeline.CreateBuilder(args);

// Parse instance info from arguments or environment
var instanceIndex = int.Parse(
    Environment.GetEnvironmentVariable("INSTANCE_INDEX") ?? "0");
var totalInstances = int.Parse(
    Environment.GetEnvironmentVariable("TOTAL_INSTANCES") ?? "1");

// Enable distributed mode
builder.AddDistributedMode(o =>
{
    o.InstanceIndex = instanceIndex;
    o.TotalInstances = totalInstances;
});

// Register the Redis coordinator
builder.AddRedisDistributedCoordinator(o =>
{
    o.ConnectionString = Environment.GetEnvironmentVariable("REDIS_URL")
        ?? "localhost:6379";
});

// Register your modules as normal
builder.Services.AddModule<BuildModule>();
builder.Services.AddModule<TestModule>();
builder.Services.AddModule<PublishModule>();

await builder.Build().RunAsync();
```

That's it. When `InstanceIndex` is `0`, the process runs as the master. All other instances run as workers.

## 3. Run Locally

Test with two terminal windows:

**Terminal 1 (Master):**
```bash
INSTANCE_INDEX=0 TOTAL_INSTANCES=2 REDIS_URL=localhost:6379 dotnet run
```

**Terminal 2 (Worker):**
```bash
INSTANCE_INDEX=1 TOTAL_INSTANCES=2 REDIS_URL=localhost:6379 dotnet run
```

The master will enqueue modules, the worker will pick them up and execute them, and results flow back to the master.

## 4. Run in CI

In GitHub Actions, use a matrix strategy to launch multiple instances. See the [GitHub Actions Example](./github-actions) for a complete workflow.

## How Run Isolation Works

Every Redis key is prefixed with a run identifier so concurrent pipeline runs on the same Redis instance don't collide. By default, the run identifier is auto-detected from CI environment variables:

1. `GITHUB_SHA` (GitHub Actions)
2. `BUILD_SOURCEVERSION` (Azure DevOps)
3. `CI_COMMIT_SHA` (GitLab CI)
4. `git rev-parse HEAD` (local git)
5. A random GUID as fallback

You can override this with an explicit value:

```csharp
builder.AddRedisDistributedCoordinator(o =>
{
    o.ConnectionString = "your-redis-url";
    o.RunIdentifier = "my-custom-run-id";
});
```

## Minimal Complete Example

Here is a self-contained pipeline with three modules:

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

builder.AddDistributedMode(o =>
{
    o.InstanceIndex = instanceIndex;
    o.TotalInstances = int.Parse(
        Environment.GetEnvironmentVariable("TOTAL_INSTANCES") ?? "1");
});

builder.AddRedisDistributedCoordinator(o =>
{
    o.ConnectionString = Environment.GetEnvironmentVariable("REDIS_URL")
        ?? "localhost:6379";
});

builder.Services.AddModule<RestoreModule>();
builder.Services.AddModule<BuildModule>();
builder.Services.AddModule<TestModule>();

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

[DependsOn<RestoreModule>]
public class BuildModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.DotNet().Build(new());
        return "built";
    }
}

[DependsOn<BuildModule>]
public class TestModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.DotNet().Test(new());
        return "tested";
    }
}
```

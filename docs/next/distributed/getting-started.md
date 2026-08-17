# Getting Started with Distributed Mode

This guide walks you through adding distributed execution to an existing ModularPipelines project using Redis as the coordinator.

## Prerequisites[​](#prerequisites "Direct link to Prerequisites")

* A working ModularPipelines pipeline.

* A Redis instance accessible to all pipeline instances. Options include:

  <!-- -->

  * [Upstash](https://upstash.com/) — free serverless Redis, great for CI.
  * [Redis Cloud](https://redis.com/cloud/) — managed Redis with a free tier.
  * Local Redis via Docker: `docker run -d -p 6379:6379 redis`

## 1. Install the Package[​](#1-install-the-package "Direct link to 1. Install the Package")

Add the `ModularPipelines.Distributed.Redis` NuGet package to your pipeline project:

```
dotnet add package ModularPipelines.Distributed.Redis
```

This brings in `ModularPipelines.Distributed` (core distributed abstractions) and `StackExchange.Redis` automatically.

## 2. Configure the Pipeline[​](#2-configure-the-pipeline "Direct link to 2. Configure the Pipeline")

In your `Program.cs`, enable distributed mode and register the Redis coordinator:

```
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

builder.AddModule<BuildModule>();

builder.AddModule<TestModule>();

builder.AddModule<PublishModule>();



await builder.ExecutePipelineAsync();
```

That's it. When `InstanceIndex` is `0`, the process runs as the master. All other instances run as workers.

## 3. Run Locally[​](#3-run-locally "Direct link to 3. Run Locally")

Test with two terminal windows:

Generate one identifier for this invocation and copy the value:

```
uuidgen
```

**Terminal 1 (Master):**

```
RUN_IDENTIFIER="paste-same-generated-uuid-here" INSTANCE_INDEX=0 TOTAL_INSTANCES=2 REDIS_URL=localhost:6379 dotnet run
```

**Terminal 2 (Worker):**

```
RUN_IDENTIFIER="paste-same-generated-uuid-here" INSTANCE_INDEX=1 TOTAL_INSTANCES=2 REDIS_URL=localhost:6379 dotnet run
```

The master will enqueue modules, the worker will pick them up and execute them, and results flow back to the master.

## 4. Run in CI[​](#4-run-in-ci "Direct link to 4. Run in CI")

In GitHub Actions, use a matrix strategy to launch multiple instances. See the [GitHub Actions Example](/ModularPipelines/docs/next/distributed/github-actions.md) for a complete workflow.

## How Run Isolation Works[​](#how-run-isolation-works "Direct link to How Run Isolation Works")

Every Redis key is prefixed with a run identifier so concurrent or repeated pipeline runs on the same Redis instance don't collide. The identifier is resolved from:

1. Explicit `RedisDistributedOptions.RunIdentifier` configuration
2. The `RUN_IDENTIFIER` environment variable

Commit hashes are not safe because repeated executions of the same commit would reuse stale keys. For local or CI multi-process runs, export one invocation-specific `RUN_IDENTIFIER` value before starting every process:

```
builder.AddRedisDistributedCoordinator(o =>

{

    o.ConnectionString = "your-redis-url";

    o.RunIdentifier = Environment.GetEnvironmentVariable("RUN_IDENTIFIER");

});
```

## Minimal Complete Example[​](#minimal-complete-example "Direct link to Minimal Complete Example")

Here is a self-contained pipeline with three modules:

```
using ModularPipelines;

using ModularPipelines.Attributes;

using ModularPipelines.Context;

using ModularPipelines.Distributed.Extensions;

using ModularPipelines.Distributed.Redis.Extensions;

using ModularPipelines.Extensions;

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



builder.AddModule<RestoreModule>();

builder.AddModule<BuildModule>();

builder.AddModule<TestModule>();



await builder.ExecutePipelineAsync();



public class RestoreModule : Module<string>

{

    protected override async Task<string> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        await context.Tools.DotNet.RestoreAsync(new());

        return "restored";

    }

}



[DependsOn<RestoreModule>]

public class BuildModule : Module<string>

{

    protected override async Task<string> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        await context.Tools.DotNet.BuildAsync(new());

        return "built";

    }

}



[DependsOn<BuildModule>]

public class TestModule : Module<string>

{

    protected override async Task<string> ExecuteAsync(

        IModuleContext context, CancellationToken cancellationToken)

    {

        await context.Tools.DotNet.TestAsync(new());

        return "tested";

    }

}
```

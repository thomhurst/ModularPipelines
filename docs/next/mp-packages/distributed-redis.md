# Distributed Redis Package

`ModularPipelines.Distributed.Redis` provides Redis-backed coordination and artifact storage for distributed pipelines.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Distributed.Redis
```

## Configuration[​](#configuration "Direct link to Configuration")

Use the combined helper when Redis should provide both services:

```
using ModularPipelines.Distributed.Redis.Extensions;

using ModularPipelines.Distributed.Extensions;



var builder = Pipeline.CreateBuilder(args);



builder.AddDistributedMode(options => options.TotalInstances = 2);

builder.AddRedisDistributed(

    redis =>

    {

        redis.ConnectionString = "localhost:6379";

        redis.RunIdentifier = Environment.GetEnvironmentVariable("RUN_IDENTIFIER")

            ?? throw new InvalidOperationException("RUN_IDENTIFIER must identify this pipeline run.");

    },

    artifacts => artifacts.TimeToLiveSeconds = 7200);
```

Set `RUN_IDENTIFIER` to the same unique value on every worker participating in one pipeline run.

`AddRedisDistributedCoordinator` and `AddRedisDistributedArtifactStore` are also available when only one Redis service is required.

## Module caching[​](#module-caching "Direct link to Module caching")

Redis can provide a shareable, cross-run module cache without enabling distributed execution:

```
builder.AddRedisModuleCache(

    redis => redis.ConnectionString = "localhost:6379",

    cacheEntries => cacheEntries.TimeToLiveSeconds = 86_400);
```

See [Cache Module Results](/ModularPipelines/docs/next/how-to/module-caching.md) for input declarations, artifact restoration, and fingerprint configuration.

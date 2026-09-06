# Distributed Redis Package

`ModularPipelines.Distributed.Redis` provides Redis-backed coordination and artifact storage for distributed pipelines.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Distributed.Redis
```

## Configuration[​](#configuration "Direct link to Configuration")

Use the combined helper when Redis should provide both services:

```
using ModularPipelines.Distributed;

using ModularPipelines.Distributed.Redis;



var builder = Pipeline.CreateBuilder(args);



builder.AddDistributedMode();

builder.AddRedisDistributed(

    redis => redis.ConnectionString = "localhost:6379",

    artifacts => artifacts.TimeToLive = TimeSpan.FromHours(2));
```

Set `MODULARPIPELINES_RUN_ID` to the same unique value on the master and every worker participating in one pipeline run. Core distributed configuration resolves `DistributedOptions.RunId` from it automatically.

`AddRedisDistributedCoordinator` and `AddRedisDistributedArtifactStore` are also available when only one Redis service is required. All Redis registration methods also accept an `IConfigurationSection` and use the .NET options pattern.

## Module caching[​](#module-caching "Direct link to Module caching")

Redis can provide a shareable, cross-run module cache without enabling distributed execution:

```
builder.AddRedisModuleCache(

    redis => redis.ConnectionString = "localhost:6379",

    cacheEntries => cacheEntries.TimeToLive = TimeSpan.FromDays(1));
```

See [Cache Module Results](/ModularPipelines/docs/next/how-to/module-caching.md) for input declarations, artifact restoration, and fingerprint configuration.

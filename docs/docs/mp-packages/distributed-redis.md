---
title: Distributed Redis Package
---

# Distributed Redis Package

`ModularPipelines.Distributed.Redis` provides Redis-backed coordination and artifact storage for distributed pipelines.

## Installation

```shell
dotnet add package ModularPipelines.Distributed.Redis
```

## Configuration

Use the combined helper when Redis should provide both services:

```csharp
using ModularPipelines.Distributed.Redis.Extensions;
using ModularPipelines.Distributed.Extensions;

var builder = Pipeline.CreateBuilder(args);

builder.AddDistributedMode();
builder.AddRedisDistributed(
    redis => redis.ConnectionString = "localhost:6379",
    artifacts => artifacts.TimeToLive = TimeSpan.FromHours(2));
```

Set `MODULARPIPELINES_RUN_ID` to the same unique value on the master and every worker participating in one pipeline run. Core distributed configuration resolves `DistributedOptions.RunId` from it automatically.

`AddRedisDistributedCoordinator` and `AddRedisDistributedArtifactStore` are also available when only one Redis service is required.
All Redis registration methods also accept an `IConfigurationSection` and use the .NET options pattern.

## Module caching

Redis can provide a shareable, cross-run module cache without enabling distributed execution:

```csharp
builder.AddRedisModuleCache(
    redis => redis.ConnectionString = "localhost:6379",
    cacheEntries => cacheEntries.TimeToLive = TimeSpan.FromDays(1));
```

See [Cache Module Results](../how-to/module-caching.md) for input declarations, artifact restoration, and fingerprint configuration.

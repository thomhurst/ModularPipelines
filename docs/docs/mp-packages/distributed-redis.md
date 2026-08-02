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

using var builder = Pipeline.CreateBuilder(args);

builder.AddDistributedMode(options => options.TotalInstances = 2);
builder.AddRedisDistributed(
    redis => redis.ConnectionString = "localhost:6379",
    artifacts => artifacts.TimeToLiveSeconds = 7200);
```

`AddRedisDistributedCoordinator` and `AddRedisDistributedArtifactStore` are also available when only one Redis service is required.

## Module caching

Redis can provide a shareable, cross-run module cache without enabling distributed execution:

```csharp
builder.AddRedisModuleCache(
    redis => redis.ConnectionString = "localhost:6379",
    cacheEntries => cacheEntries.TimeToLiveSeconds = 86_400);
```

See [Cache Module Results](../how-to/module-caching.md) for input declarations, artifact restoration, and fingerprint configuration.

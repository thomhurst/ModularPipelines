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

builder.AddDistributedMode(options => options.TotalInstances = 2);
builder.AddRedisDistributed(
    redis => redis.ConnectionString = "localhost:6379",
    artifacts => artifacts.TimeToLiveSeconds = 7200);
```

`AddRedisDistributedCoordinator` and `AddRedisDistributedArtifactStore` are also available when only one Redis service is required.

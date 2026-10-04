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
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Redis;

var builder = Pipeline.CreateBuilder(args);

builder.AddDistributedMode();
builder.AddRedisDistributed(redis =>
{
    redis.ConnectionString = "localhost:6379";
    redis.TimeToLive = TimeSpan.FromHours(2);
});
```

`RedisOptions.TimeToLive` applies to every Redis key, including artifacts, and must exceed
`DistributedOptions.ModuleResultTimeout`. Options are validated when the pipeline is built.

Set credentials that a connection string cannot carry safely, such as a password containing commas, through
`ConfigureConnection`:

```csharp
builder.AddRedisDistributed(redis =>
{
    redis.ConnectionString = "redis.example.com:6380";
    redis.ConfigureConnection = connection =>
    {
        connection.Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD");
        connection.Ssl = true;
        connection.AbortOnConnectFail = false;
    };
});
```

Each Redis feature owns its connection and connects asynchronously on first use. The package does not register
or reuse an `IConnectionMultiplexer` from the service collection.

Set `MODULARPIPELINES_RUN_ID` to the same unique value on the master and every worker participating in one pipeline run. Core distributed configuration resolves `DistributedOptions.RunId` from it automatically.

`AddRedisDistributedCoordinator` and `AddRedisDistributedArtifactStore` are also available when only one Redis service is required.
Every Redis registration method has one `Action<RedisOptions>` overload and one `IConfigurationSection` overload.
Backend-independent artifact settings, such as `ArtifactCompressionLevel`, are configured once through `DistributedOptions`:

```csharp
builder.Services.Configure<DistributedOptions>(options => options.ArtifactCompressionLevel = CompressionLevel.Optimal);
```

See [Configuration](../distributed/configuration.md#redisoptions) for every `RedisOptions` property and the key schema.

## Module caching

Redis can provide a shareable, cross-run module cache without enabling distributed execution:

```csharp
builder.AddRedisModuleCache(redis =>
{
    redis.ConnectionString = "localhost:6379";
    redis.TimeToLive = TimeSpan.FromDays(1);
});
```

The module cache has its own `RedisOptions` and connection, separate from the distributed coordinator and artifact
store, so it can point at a different Redis server. Its keys use the fingerprint as a Redis Cluster hash tag.

See [Cache Module Results](../how-to/module-caching.md) for input declarations, artifact restoration, and fingerprint configuration.

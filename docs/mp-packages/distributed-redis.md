# Distributed Redis Package

`ModularPipelines.Distributed.Redis` provides Redis-backed coordination and artifact storage for distributed pipelines.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Distributed.Redis
```

The coordinator needs **Redis 7.4 or later, or Valkey 9.0 or later**, because worker heartbeats use hash field expiry (`HPEXPIRE`). Creating a coordinator against an older server fails with a `NotSupportedException` that names the requirement. The artifact store and module cache work with older servers.

## Configuration[​](#configuration "Direct link to Configuration")

Use the combined helper when Redis should provide both services:

```
using ModularPipelines.Distributed;

using ModularPipelines.Distributed.Redis;



var builder = Pipeline.CreateBuilder(args);



builder.AddDistributedMode();

builder.AddRedisDistributed(redis => redis with

{

    ConnectionString = "localhost:6379",

    TimeToLive = TimeSpan.FromHours(2),

});
```

`RedisOptions.TimeToLive` applies to every Redis key, including artifacts. For the unnamed run options, startup validation requires it to exceed both `DistributedOptions.ModuleResultTimeout` and `DistributedOptions.MasterTimeout`; coordinator creation also requires it to exceed `DistributedOptions.WorkerTimeout`. An infinite module result timeout does not disable key expiration. Named module-cache options require a positive TTL but are not compared with distributed timeouts.

`RedisOptions.FailOnUnsafeMemoryPolicy` defaults to `false`: coordinator and artifact-store startup logs a warning and continues when `INFO memory` reports finite `maxmemory` with an evicting policy, or when diagnostics cannot establish the configuration. Set it to `true` to fail startup with `InvalidOperationException` instead. Use `noeviction` or unlimited `maxmemory` to prevent early eviction of run state and artifact chunks. The check needs `INFO` permission, observes only the server answering the command rather than every Redis Cluster node, and does not guard against later configuration changes. Module-cache connections are not checked.

Set credentials that a connection string cannot carry safely, such as a password containing commas, through `ConfigureConnection`:

```
builder.AddRedisDistributed(redis => redis with

{

    ConnectionString = "redis.example.com:6380",

    ConfigureConnection = connection => connection with

    {

        Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD"),

        UseTls = true,

    },

});
```

The package uses the [Respire](https://thomhurst.github.io/Respire/) client. `ConfigureConnection` transforms the immutable `Respire.RespireOptions` parsed from the connection string. Each Redis feature owns its connection and connects asynchronously on first use. The package does not register or reuse an `IRespireClient` from the service collection.

Set `MODULARPIPELINES_RUN_ID` to the same unique value on the master and every worker participating in one pipeline run. Core distributed configuration resolves `DistributedOptions.RunId` from it automatically.

`AddRedisDistributedCoordinator` and `AddRedisDistributedArtifactStore` are also available when only one Redis service is required. These methods and `AddRedisDistributed` each accept either a `Func<RedisOptions, RedisOptions>` transform or an `IConfigurationSection`. `AddRedisModuleCache` has the four overloads described below. Backend-independent artifact settings, such as `ArtifactCompressionLevel`, are configured once through `DistributedOptions`:

```
builder.ConfigureDistributedOptions(options => options with { ArtifactCompressionLevel = CompressionLevel.Optimal });
```

See [Configuration](/ModularPipelines/docs/distributed/configuration.md#redisoptions) for every `RedisOptions` property and the key schema.

## Module caching[​](#module-caching "Direct link to Module caching")

Redis can provide a shareable, cross-run module cache without enabling distributed execution:

```
builder.AddRedisModuleCache(redis => redis with

{

    ConnectionString = "localhost:6379",

    TimeToLive = TimeSpan.FromDays(1),

});
```

The module cache has its own `RedisOptions` and connection, separate from the distributed coordinator and artifact store, so it can point at a different Redis server. Its keys use the fingerprint as a Redis Cluster hash tag.

`AddRedisModuleCache` has four overloads, with these arguments after the builder:

| Arguments                                                                                                  | Configuration                                               |
| ---------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------- |
| `Func<RedisOptions, RedisOptions> configure`                                                               | Redis options transform.                                    |
| `Func<RedisOptions, RedisOptions> configure, Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache` | Redis and module-cache options transforms.                  |
| `IConfigurationSection section`                                                                            | Redis options bound from configuration.                     |
| `IConfigurationSection section, Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache`              | Redis options binding and a module-cache options transform. |

The overloads without `configureCache` use the default module-cache settings.

See [Cache Module Results](/ModularPipelines/docs/how-to/module-caching.md) for input declarations, artifact restoration, and fingerprint configuration.

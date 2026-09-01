---
title: Configuration
sidebar_position: 3
---

# Configuration

Distributed mode has two layers of configuration: the core `DistributedOptions` (shared across all coordinator implementations) and coordinator-specific options like `RedisDistributedOptions`.

## DistributedOptions

Passed to `AddDistributedMode()`. Controls the fundamental behavior of the master/worker system.

```csharp
builder.AddDistributedMode(o =>
{
    o.InstanceIndex = 0;
    o.TotalInstances = 4;
    o.Capabilities = [Capability.Docker, Capability.Gpu];
    o.RunIdentifier = Environment.GetEnvironmentVariable("RUN_IDENTIFIER");
    o.CapabilityTimeout = TimeSpan.FromMinutes(5);
    o.ModuleResultTimeout = TimeSpan.FromMinutes(45);
    o.AutoDetectOsCapability = true;
});
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `InstanceIndex` | `int` | `0` | This instance's index. `0` = master, `> 0` = worker. Can be overridden by the `MODULAR_PIPELINES_INSTANCE` environment variable. |
| `TotalInstances` | `int` | `1` | Total number of instances (master + workers). |
| `Capabilities` | `IReadOnlyList<Capability>` | `[]` | Capabilities this worker advertises. Built-in values are available from `Capability`; strings convert implicitly for custom values. |
| `RunIdentifier` | `string?` | `null` | Identifier shared by every process in this pipeline run. |
| `CapabilityTimeout` | `TimeSpan` | `TimeSpan.FromMinutes(5)` | Maximum time to wait for worker registration before distributing work among the available workers. |
| `ModuleResultTimeout` | `TimeSpan` | `TimeSpan.FromMinutes(45)` | Default maximum time to wait for a distributed module result. Use `TimeSpan.Zero` to wait indefinitely. |
| `AutoDetectOsCapability` | `bool` | `true` | Automatically add the current OS as a capability (`"windows"`, `"linux"`, or `"macos"`). |

### Configuration from appsettings.json

You can also bind from configuration:

```json
{
  "Distributed": {
    "InstanceIndex": 0,
    "TotalInstances": 4,
    "Capabilities": ["docker"],
    "CapabilityTimeout": "00:05:00"
  }
}
```

```csharp
builder.AddDistributedMode(builder.Configuration.GetSection("Distributed"));
```

Configuration binding converts the string array to `Capability` values. Distributed wire payloads also remain plain JSON strings.

## RedisDistributedOptions

Passed to `AddRedisDistributedCoordinator()`. Controls how the Redis coordinator connects and manages keys.

```csharp
builder.AddRedisDistributedCoordinator(o =>
{
    o.ConnectionString = "redis-host:6379,password=secret";
    o.RunIdentifier = Environment.GetEnvironmentVariable("RUN_IDENTIFIER");
    o.KeyPrefix = "modpipe";
    o.KeyExpirationSeconds = 3600;
});
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ConnectionString` | `string` | `""` | StackExchange.Redis connection string. Supports all standard options (`password`, `ssl`, `abortConnect`, etc.). **Required.** |
| `RunIdentifier` | `string?` | `null` | Unique identifier for this pipeline execution. Used to isolate Redis keys so concurrent or repeated runs don't collide. If `null`, `RUN_IDENTIFIER` is read; otherwise configuration fails. |
| `KeyPrefix` | `string` | `"modpipe"` | Prefix for all Redis keys. Change this if multiple different pipelines share the same Redis instance. |
| `KeyExpirationSeconds` | `int` | `3600` | TTL in seconds for all Redis keys. Keys are automatically cleaned up after this duration. |

## Run Identifier Resolution

Distributed coordination requires an invocation-scoped identifier. It is resolved in this order:

| Priority | Source | Environment |
|----------|--------|-------------|
| 1 | `RedisDistributedOptions.RunIdentifier` | Explicit configuration |
| 2 | `RUN_IDENTIFIER` env var | Any CI or local orchestration |

Commit identifiers are deliberately not accepted: rerunning the same commit must receive a fresh
Redis namespace. Local multi-process runs should export one unique `RUN_IDENTIFIER` value before
starting the master and workers. CI workflows must likewise generate or derive one invocation-specific
value and export it as `RUN_IDENTIFIER` for every master and worker.

## Redis Key Schema

All keys follow the pattern `{KeyPrefix}:{RunIdentifier}:{purpose}`. With the defaults, keys look like:

| Key | Redis Type | Purpose |
|-----|-----------|---------|
| `modpipe:{run}:work:queue` | List | FIFO work queue for module assignments |
| `modpipe:{run}:results` | Hash | Completed module results (field = module type name) |
| `modpipe:{run}:workers` | Hash | Registered worker information (field = worker index) |
| `modpipe:{run}:heartbeats` | Hash | Worker heartbeat timestamps (field = worker index) |
| `modpipe:{run}:cancellation` | String | Cancellation signal (set when cancellation is broadcast) |

Pub/Sub channels (no TTL, ephemeral):

| Channel | Purpose |
|---------|---------|
| `modpipe:{run}:results:{ModuleTypeName}` | Notifies the master when a specific module's result is ready |
| `modpipe:{run}:cancellation:signal` | Notifies all instances of a cancellation request |

All storage keys have the configured TTL applied, so they are automatically cleaned up even if the pipeline crashes.

## Connection String Examples

**Local Redis:**
```
localhost:6379
```

**Redis with password:**
```
redis-host:6379,password=mysecret
```

**Redis with TLS (e.g., Upstash, Redis Cloud):**
```
redis-host:6380,password=mysecret,ssl=True,abortConnect=False
```

**Multiple endpoints (Redis Cluster):**
```
host1:6379,host2:6379,password=mysecret
```

See the [StackExchange.Redis configuration docs](https://stackexchange.github.io/StackExchange.Redis/Configuration.html) for all connection string options.

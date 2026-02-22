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
    o.Capabilities = new List<string> { "docker", "gpu" };
    o.HeartbeatIntervalSeconds = 10;
    o.HeartbeatTimeoutSeconds = 30;
    o.AutoDetectOsCapability = true;
});
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `InstanceIndex` | `int` | `0` | This instance's index. `0` = master, `> 0` = worker. Can be overridden by the `MODULAR_PIPELINES_INSTANCE` environment variable. |
| `TotalInstances` | `int` | `1` | Total number of instances (master + workers). |
| `Capabilities` | `IList<string>` | `[]` | Capabilities this worker advertises. Modules with `[RequiresCapability]` will only be assigned to workers that have matching capabilities. |
| `HeartbeatIntervalSeconds` | `int` | `10` | How often workers send heartbeat signals (seconds). |
| `HeartbeatTimeoutSeconds` | `int` | `30` | How long before the master considers a worker unresponsive (seconds). |
| `CapabilityTimeoutSeconds` | `int` | `300` | Maximum time to wait for a capable worker to become available before failing a module (seconds). |
| `AutoDetectOsCapability` | `bool` | `true` | Automatically add the current OS as a capability (`"windows"`, `"linux"`, or `"macos"`). |

### Configuration from appsettings.json

You can also bind from configuration:

```json
{
  "Distributed": {
    "InstanceIndex": 0,
    "TotalInstances": 4,
    "Capabilities": ["docker"],
    "HeartbeatIntervalSeconds": 15
  }
}
```

```csharp
builder.AddDistributedMode(builder.Configuration.GetSection("Distributed"));
```

## RedisDistributedOptions

Passed to `AddRedisDistributedCoordinator()`. Controls how the Redis coordinator connects and manages keys.

```csharp
builder.AddRedisDistributedCoordinator(o =>
{
    o.ConnectionString = "redis-host:6379,password=secret";
    o.RunIdentifier = null;          // auto-detect
    o.KeyPrefix = "modpipe";
    o.KeyExpirationSeconds = 3600;
    o.DequeuePollDelayMilliseconds = 100;
});
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ConnectionString` | `string` | `""` | StackExchange.Redis connection string. Supports all standard options (`password`, `ssl`, `abortConnect`, etc.). **Required.** |
| `RunIdentifier` | `string?` | `null` | Unique identifier for this pipeline run. Used to isolate Redis keys so concurrent runs don't collide. If `null`, auto-detected (see below). |
| `KeyPrefix` | `string` | `"modpipe"` | Prefix for all Redis keys. Change this if multiple different pipelines share the same Redis instance. |
| `KeyExpirationSeconds` | `int` | `3600` | TTL in seconds for all Redis keys. Keys are automatically cleaned up after this duration. |
| `DequeuePollDelayMilliseconds` | `int` | `100` | Delay between dequeue poll attempts when the work queue is empty. Lower values mean faster pickup but more Redis traffic. |

## Run Identifier Resolution

When `RunIdentifier` is not set explicitly, it is resolved automatically in this order:

| Priority | Source | Environment |
|----------|--------|-------------|
| 1 | `RedisDistributedOptions.RunIdentifier` | Explicit configuration |
| 2 | `GITHUB_SHA` env var | GitHub Actions |
| 3 | `BUILD_SOURCEVERSION` env var | Azure DevOps |
| 4 | `CI_COMMIT_SHA` env var | GitLab CI |
| 5 | `git rev-parse HEAD` | Any git repository |
| 6 | `Guid.NewGuid()` | Fallback |

This means in most CI environments, the run identifier is the commit SHA, which naturally isolates concurrent runs on the same Redis instance.

## Redis Key Schema

All keys follow the pattern `{KeyPrefix}:{RunIdentifier}:{purpose}`. With the defaults, keys look like:

| Key | Redis Type | Purpose |
|-----|-----------|---------|
| `modpipe:{sha}:work:queue` | List | FIFO work queue for module assignments |
| `modpipe:{sha}:results` | Hash | Completed module results (field = module type name) |
| `modpipe:{sha}:workers` | Hash | Registered worker information (field = worker index) |
| `modpipe:{sha}:heartbeats` | Hash | Worker heartbeat timestamps (field = worker index) |
| `modpipe:{sha}:cancellation` | String | Cancellation signal (set when cancellation is broadcast) |

Pub/Sub channels (no TTL, ephemeral):

| Channel | Purpose |
|---------|---------|
| `modpipe:{sha}:results:{ModuleTypeName}` | Notifies the master when a specific module's result is ready |
| `modpipe:{sha}:cancellation:signal` | Notifies all instances of a cancellation request |

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

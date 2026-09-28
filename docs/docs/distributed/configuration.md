---
title: Configuration
sidebar_position: 3
---

# Configuration

Distributed mode has two layers of configuration: the core `DistributedOptions` (shared across all coordinator implementations) and backend-specific options like `RedisOptions`.

## Module identity and build compatibility

Assignments, stored results, dependency references, artifact descriptors, and worker command counts use `ModuleId`. The value is case-sensitive and serializes as a JSON string. By default it derives from the module's full type name; generic arguments omit assembly versions. Assign an explicit ID to preserve identity when renaming or moving a module:

```csharp
using ModularPipelines.Attributes;

[ModuleId("build.application")]
public class BuildModule : Module<string>
{
    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult("output");
}
```

Each registered module must have a unique ID. Cache fingerprints also use this identity; existing assembly-version cache settings still control cache invalidation. This changes the fingerprint format, so existing caches are refreshed once.

Workers publish `PipelineSchemaVersion` during registration, and masters include it in every assignment. The stamp includes the wire schema, registered module IDs, result type identities, and application assembly build IDs throughout module inheritance and result contracts, including generic arguments, element types, implemented collection interfaces, base types, serialized property and field types, attributed JSON converters, and polymorphic contracts declared with `JsonDerivedType`. Run the same pipeline binaries on every participant. Missing or mismatched stamps produce a schema mismatch error before module execution; they do not silently execute against a different build. A stable module ID does not bypass this build compatibility check.

Shared .NET framework assemblies contribute stable type names instead of assembly build IDs, so shared-runtime servicing updates do not invalidate the stamp. Application types inside framework containers such as `List<MyResult>` still require matching builds, as do application-provided replacements for framework assemblies. Self-contained deployments and custom hosts without shared-framework metadata should deploy matching runtime binaries as well.

The wire DTO changes are breaking: upgrade masters, workers, and custom coordinators together. `WaitForResultAsync` accepts `ModuleId`, and `SerializedModuleResult` resolves its result type through the local registry rather than a remote result type name.

## DistributedOptions

Passed to `AddDistributedMode()`. Controls the fundamental behavior of the master/worker system.

```csharp
builder.AddDistributedMode(o =>
{
    o.InstanceIndex = 0;
    o.TotalInstances = 4;
    o.MaxParallelism = 4;
    o.Role = DistributedRole.Master;
    o.Capabilities = [Capability.Docker, Capability.Gpu];
    o.RunId = Environment.GetEnvironmentVariable("MODULARPIPELINES_RUN_ID")!;
    o.CapabilityTimeout = TimeSpan.FromMinutes(5);
    o.MinimumWorkerCount = 0;
    o.ModuleResultTimeout = TimeSpan.FromMinutes(45);
});
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Role` | `DistributedRole` | `Auto` | Explicit `Master` or `Worker` role. `Auto` derives the role from `InstanceIndex`. |
| `InstanceIndex` | `int` | `0` | This instance's unique index. With `Role == Auto`, `0` selects master and values above `0` select worker. |
| `TotalInstances` | `int` | `1` | Total number of instances (master + workers). |
| `MaxParallelism` | `int?` | `null` | Optional per-node concurrency limit. It can lower, but cannot raise, the pipeline's global `Concurrency.MaxParallelism` limit. |
| `Capabilities` | `IReadOnlyList<Capability>` | `[]` | Capabilities this instance advertises in addition to those detected by registered `ICapabilityProvider` services, including the current OS. Built-in values are available from `Capability`; strings convert implicitly for custom values. |
| `RunId` | `string` | `MODULARPIPELINES_RUN_ID` or generated for one instance | Identifier shared by every process in this pipeline run. Multi-instance runs fail fast when neither source is configured. |
| `RequireExplicitRunId` | `bool` | `false` | Reject generated single-instance IDs. Shared Redis backends enable this automatically. S3 artifact-store registrations also require an explicit shared `RunId`, including for single-instance configurations. Custom backends opt in with `builder.RequireExplicitRunId()`. |
| `CapabilityTimeout` | `TimeSpan` | `TimeSpan.FromMinutes(5)` | Registration grace period before an assignment with no capable worker fails with an explicit routing error. |
| `MinimumWorkerCount` | `int` | `0` | Number of external workers required before dispatch starts. Keep zero for immediate dispatch; set `TotalInstances - 1` for the former full-worker barrier. |
| `ModuleResultTimeout` | `TimeSpan` | `TimeSpan.FromMinutes(45)` | Default maximum time to wait for a distributed module result. Use `TimeSpan.Zero` to wait indefinitely. |

### Configuration from appsettings.json

You can also bind from configuration:

```json
{
  "Distributed": {
    "InstanceIndex": 0,
    "TotalInstances": 4,
    "RunId": "unique-invocation-id",
    "Capabilities": ["docker"],
    "CapabilityTimeout": "00:05:00",
    "MinimumWorkerCount": 0
  }
}
```

```csharp
builder.AddDistributedMode(builder.Configuration.GetSection("Distributed"));
```

Or call `builder.AddDistributedMode()` to bind the standard environment variables:

| Environment variable | Option |
|----------------------|--------|
| `MODULARPIPELINES_INSTANCE_INDEX` | `InstanceIndex` |
| `MODULARPIPELINES_TOTAL_INSTANCES` | `TotalInstances` |
| `MODULARPIPELINES_MAX_PARALLELISM` | `MaxParallelism` |
| `MODULARPIPELINES_RUN_ID` | `RunId` |
| `MODULARPIPELINES_ROLE` | `Role` (`Auto`, `Master`, or `Worker`) |

Configuration binding converts the string array to `Capability` values. Distributed wire payloads also remain plain JSON strings.

Satellite registrations use the same options pattern and accept configuration sections:

```csharp
builder.AddRedisDistributedCoordinator(builder.Configuration.GetSection("Redis"));
builder.AddSignalRDistributedCoordinator(builder.Configuration.GetSection("SignalR"));
builder.AddRedisMasterDiscovery(builder.Configuration.GetSection("RedisDiscovery"));
builder.AddS3DistributedArtifactStore(builder.Configuration.GetSection("S3"));
```

`AddRedisMasterDiscovery` keys the advertised master endpoint by `DistributedOptions.RunId` and enables
`RequireExplicitRunId`, so every process in the run must share one explicit `RunId`: set it in the
`Distributed` section, assign `options.RunId`, or export `MODULARPIPELINES_RUN_ID` for each master and
worker. See [Run Identifier Resolution](#run-identifier-resolution).

Each backend registration has exactly two overloads: one taking an `Action<TOptions>` and one taking an
`IConfigurationSection`. Only one coordinator backend and one artifact store backend can be registered;
registering a second, different backend throws `InvalidOperationException` instead of silently replacing
the first. Registering the same backend again is a no-op apart from applying the extra configuration.

## ArtifactOptions

`ArtifactOptions` holds the artifact settings that apply to every artifact store. Configure them once with the
options pattern, independently of the backend:

```csharp
builder.Services.Configure<ArtifactOptions>(o => o.CompressionLevel = CompressionLevel.Optimal);
// or: builder.Services.Configure<ArtifactOptions>(builder.Configuration.GetSection("Artifacts"));
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `CompressionLevel` | `CompressionLevel` | `Fastest` | Compression level for directory artifacts. |

Storage-specific settings such as expiry, chunk size and multipart part size live on the backend's options
(`RedisOptions`, `S3StorageOptions`).

## RedisOptions

Passed to `AddRedisDistributedCoordinator()`, `AddRedisDistributedArtifactStore()`, `AddRedisDistributed()` and
`AddRedisModuleCache()`. Controls how the Redis features connect and manage keys. The coordinator and artifact
store share one `RedisOptions` instance and one connection; the module cache has its own, so it can use a
different Redis server.

```csharp
builder.AddRedisDistributed(o =>
{
    o.ConnectionString = "redis-host:6379";
    o.ConfigureConnection = connection =>
    {
        connection.Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD");
        connection.Ssl = true;
    };
    o.KeyPrefix = "modpipe";
    o.TimeToLive = TimeSpan.FromHours(1);
});
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ConnectionString` | `string` | `""` | StackExchange.Redis connection string. Supports all standard options (`password`, `ssl`, `abortConnect`, etc.). **Required** unless `ConfigureConnection` supplies the endpoints. |
| `ConfigureConnection` | `Action<ConfigurationOptions>?` | `null` | Adjusts the parsed `ConfigurationOptions` before connecting. Use it for passwords containing commas or other characters a connection string cannot carry, TLS, and retry settings. |
| `KeyPrefix` | `string` | `"modpipe"` | Prefix for all Redis keys. Change this if multiple different pipelines share the same Redis instance. |
| `TimeToLive` | `TimeSpan` | `TimeSpan.FromHours(1)` | TTL for all Redis keys, including artifacts and module cache entries. Keys are automatically cleaned up after this duration. |
| `ChunkSizeBytes` | `int` | 4 MB | Size of each Redis value used for artifacts and module cache entries. Content that fits in one chunk is stored under a single key. |

Each registration owns its connection and connects asynchronously on first use. The package never registers or
resolves an `IConnectionMultiplexer` from the service collection, so an application's own multiplexer is not
adopted and cannot conflict with it.

The options are validated when the pipeline is built. A missing connection, an empty `KeyPrefix`, or a
non-positive `TimeToLive` or `ChunkSizeBytes` fails fast with `OptionsValidationException`. For the coordinator
and artifact store, `TimeToLive` must also exceed `DistributedOptions.ModuleResultTimeout` so run keys and
artifacts cannot expire mid-run.

All distributed duration properties use `TimeSpan`. When binding them from `appsettings.json`, use the invariant `TimeSpan` string format:

```json
{
  "Distributed": {
    "CapabilityTimeout": "00:05:00",
    "ModuleResultTimeout": "00:45:00"
  },
  "Redis": {
    "TimeToLive": "01:00:00"
  },
  "SignalR": {
    "ConnectionTimeout": "00:02:00",
    "ReconnectGrace": "00:00:45",
    "KeepAliveInterval": "00:00:05",
    "PeerTimeout": "00:00:15",
    "TunnelStartupTimeout": "00:00:30"
  },
  "RedisDiscovery": {
    "Ttl": "01:00:00",
    "DiscoveryTimeout": "00:02:00",
    "PollInterval": "00:00:00.500"
  }
}
```

## Run Identifier Resolution

Distributed coordination requires an invocation-scoped identifier. It is resolved in this order:

| Priority | Source | Environment |
|----------|--------|-------------|
| 1 | `DistributedOptions.RunId` | Explicit configuration |
| 2 | `MODULARPIPELINES_RUN_ID` env var | Any CI or local orchestration |
| 3 | Generated GUID | Single-process/default fallback |

Use an invocation-specific value rather than a stable commit identifier so rerunning the same commit
receives a fresh Redis namespace. Local multi-process runs should export one unique `MODULARPIPELINES_RUN_ID`
value before starting the master and workers. CI workflows must likewise generate or derive one
invocation-specific value and export it as `MODULARPIPELINES_RUN_ID` for every master and worker.

## Redis Key Schema

All keys follow the pattern `{KeyPrefix}:{RunId}:{purpose}`. With the defaults, keys look like:

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
| `modpipe:{run}:results:{ModuleId}` | Notifies the master when a specific module's result is ready |
| `modpipe:{run}:cancellation:signal` | Notifies all instances of a cancellation request |

Artifacts are stored under the same run namespace:

| Key | Redis Type | Purpose |
|-----|-----------|---------|
| `modpipe:{run}:artifacts:meta:{id}` | String | Artifact metadata (JSON) |
| `modpipe:{run}:artifacts:data:{id}` | String | Content of an artifact that fits in one chunk (including empty artifacts) |
| `modpipe:{run}:artifacts:data:{id}:chunk:{n}` | String | Chunks of a larger artifact |
| `modpipe:{run}:artifacts:index:{ModuleId}` | Set | Artifact ids uploaded by a module |

The braces around the run identifier form a Redis Cluster hash tag, so every key of a run maps to one slot.
Module cache entries are not run-scoped; they use `{KeyPrefix}:module-cache:v2:{fingerprint}:...`, where the
braced fingerprint is the hash tag that keeps an entry's metadata and chunks in one slot.

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

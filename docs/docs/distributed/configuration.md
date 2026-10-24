---
title: Configuration
sidebar_position: 3
---

# Configuration

Distributed mode has two layers of configuration: the core `DistributedOptions` (shared across all coordinator implementations) and backend-specific options like `RedisOptions`.

## Module identity and build compatibility

Assignments, stored results, dependency references, artifact descriptors, and worker command counts use `ModuleId`. The value is case-sensitive and serializes as a JSON string. By default it includes the module's full type name and simple assembly name; assembly versions are omitted, including in generic arguments. Assign an explicit ID to preserve identity when renaming or moving a module:

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

All distributed and backend options are immutable records. Return updated options from
configuration callbacks using `options => options with { ... }`. Update nested tunnel
options with `options.Tunnel with { ... }`. Transforms and configuration-section bindings
apply in registration order; startup validation runs after configuration and post-configuration.
Named module-cache options remain separate from distributed backend options.

Use `builder.ConfigureDistributedOptions(options => options with { ArtifactCompressionLevel =
CompressionLevel.NoCompression })` to configure artifact behavior in local mode. This does
not enable distributed execution. Redis `ConfigureConnection` options are
`Func<RespireOptions, RespireOptions>` transforms over the immutable Respire connection options.

Both Redis integration packages remain beta independently of core v4. Their `ConfigureConnection`
callbacks intentionally expose pre-1.0 Respire types, so package upgrades may require callback changes.
This beta status is separate from core's versioning policy, which allows breaking changes in minor
versions. See the compatibility policy for [Redis](../mp-packages/distributed-redis.md) and
[Redis discovery](../mp-packages/distributed-discovery-redis.md) before upgrading.

```csharp
builder.AddDistributedMode(o => o with
{
    InstanceIndex = 0,
    TotalInstances = 4,
    MaxParallelism = 4,
    Role = DistributedRole.Master,
    Capabilities = [Capability.Docker, Capability.Gpu],
    RunId = Environment.GetEnvironmentVariable("MODULARPIPELINES_RUN_ID")!,
    WorkerRegistrationTimeout = TimeSpan.FromMinutes(5),
    WorkerHeartbeatInterval = TimeSpan.FromSeconds(5),
    WorkerTimeout = TimeSpan.FromSeconds(30),
    MasterTimeout = TimeSpan.FromMinutes(1),
    MinimumWorkerCount = 0,
    ModuleResultTimeout = TimeSpan.FromMinutes(45),
});
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Role` | `DistributedRole` | `Auto` | Explicit `Master` or `Worker` role. `Auto` derives the role from `InstanceIndex`. |
| `InstanceIndex` | `int` | `0` | This instance's unique index, less than `TotalInstances`. With `Role == Auto`, `0` selects master and values above `0` select worker. Each process's `WorkerId` is `instance-{InstanceIndex}`. |
| `TotalInstances` | `int` | `1` | Total number of instances (master + workers). Values above `1` require a shared coordinator backend. |
| `MaxParallelism` | `int?` | `null` | Optional per-node concurrency limit. It can lower, but cannot raise, the pipeline's global `Concurrency.MaxParallelism` limit. Workers advertise it when they register. |
| `Capabilities` | `IReadOnlyList<Capability>` | `[]` | Capabilities this instance advertises in addition to those detected by registered `ICapabilityProvider` services, including the current OS. Built-in values are available from `Capability`; create custom values with `new Capability("name")`. |
| `RunId` | `string` | `MODULARPIPELINES_RUN_ID` or generated for one instance | Identifier shared by every process in this pipeline run: 1-128 ASCII letters, digits, `.`, `_` or `-`, because backends embed it in keys and URLs. Multi-instance runs fail fast when neither source is configured. |
| `RequireExplicitRunId` | `bool` | `false` | Reject generated single-instance IDs. The Redis and SignalR coordinators and Redis discovery enable this automatically. S3 artifact-store registrations also require an explicit shared `RunId`, including for single-instance configurations. Custom backends opt in with `builder.RequireExplicitRunId()`. |
| `WorkerRegistrationTimeout` | `TimeSpan` | `TimeSpan.FromMinutes(5)` | How long the master waits for workers to register: for `MinimumWorkerCount` before dispatch starts, and for a capable worker before an assignment the master cannot run fails with a routing error. |
| `WorkerHeartbeatInterval` | `TimeSpan` | `TimeSpan.FromSeconds(5)` | How often workers, and the master's own worker loop, report liveness and renew the leases on their in-flight modules. |
| `WorkerTimeout` | `TimeSpan` | `TimeSpan.FromSeconds(30)` | How long a registration and its leases stay valid without a heartbeat. The master requeues expired leases until `MaxModuleLeaseExpirations` is reached, then fails the module. Must exceed `WorkerHeartbeatInterval`. |
| `MaxModuleLeaseExpirations` | `int` | `2` | Number of lease expirations that permanently fail a module. Allows one recovery by default; `1` fails on the first expiration. Must be at least `1`. The budget applies per module across all workers, independently of module retries. |
| `MasterTimeout` | `TimeSpan` | `TimeSpan.FromMinutes(1)` | How long workers keep running after the master stops sending heartbeats without having signalled completion, or while they cannot reach the coordinator to check. Workers then cancel their in-flight modules, including AlwaysRun modules, and fail. SignalR workers detect a lost master when their connection closes for good instead. Must exceed `WorkerHeartbeatInterval`. |
| `MinimumWorkerCount` | `int` | `0` | Number of external workers required before dispatch starts, between zero and `TotalInstances - 1`. Keep zero for immediate dispatch. |
| `ModuleResultTimeout` | `TimeSpan` | `TimeSpan.FromMinutes(45)` | The master's backstop, measured from when a worker claims the module. With a finite per-attempt timeout, it allows every configured attempt at that timeout plus this period. With an infinite or unconfigured effective timeout, this period is the entire deadline. `Timeout.InfiniteTimeSpan` disables this backstop; worker timeouts remain independent. Zero and other negative values are rejected. See [Module result deadlines](#module-result-deadlines). |
| `RunCleanup` | `DistributedRunCleanupPolicy` | `Always` | Which pipeline outcomes delete the data backends stored for the run. See [Run data cleanup](#run-data-cleanup). |
| `RunCleanupTimeout` | `TimeSpan` | `TimeSpan.FromMinutes(2)` | How long cleanup waits for workers to publish their final status, and how long each backend may take to delete the run's data. Cleanup is skipped when a worker does not finish in time. |

Invalid combinations, such as `InstanceIndex >= TotalInstances`, `WorkerTimeout <= WorkerHeartbeatInterval`, `MasterTimeout <= WorkerHeartbeatInterval` or `MaxModuleLeaseExpirations < 1`,
are reported by pipeline validation and stop the distributed backend from starting.

### Module result deadlines

The worker enforces the module's per-attempt timeout. Separately, the master uses
`ModuleResultTimeout` to protect against a claimed module that never returns a result.
The master's deadline starts when a worker claims the module; time spent waiting in the
queue does not consume it.

For a finite per-attempt timeout, the master allows `(1 + retry count) × per-attempt timeout`
plus `ModuleResultTimeout`. When the effective per-attempt timeout is `Timeout.InfiniteTimeSpan`,
or neither the module nor the pipeline supplies a timeout, `ModuleResultTimeout` is the entire
deadline. With its default value, the master therefore times out a claimed module after
45 minutes even when the worker's module timeout is disabled. An unset module timeout inherits
`PipelineOptions.DefaultModuleTimeout`, which defaults to 30 minutes; it does not disable timeouts.

To allow execution without either timeout limit, disable both the module timeout and the
master's result backstop:

```csharp
builder.ConfigureOptions(options => options with
{
    DefaultModuleTimeout = Timeout.InfiniteTimeSpan,
});
builder.AddDistributedMode(options => options with
{
    ModuleResultTimeout = Timeout.InfiniteTimeSpan,
});
```

Per-module overrides still apply. Remove finite overrides or use
`module.WithTimeout(Timeout.InfiniteTimeSpan)` in the module's `Configure` method; see
[Timeouts](../how-to/timeouts.md). Setting only `ModuleResultTimeout` to infinite removes the
master's deadline but leaves the worker's per-attempt timeout in force.

Worker and master liveness checks remain active (`WorkerTimeout`, `MasterTimeout`, and
backend connection checks). Backend retention limits are also independent: for example,
Redis keys retain their configured `TimeToLive`, so choose a TTL that covers the required
retention period even when `ModuleResultTimeout` is infinite.

### Configuration from appsettings.json

You can also bind from configuration:

```json
{
  "Distributed": {
    "InstanceIndex": 0,
    "TotalInstances": 4,
    "RunId": "unique-invocation-id",
    "Capabilities": ["docker"],
    "WorkerRegistrationTimeout": "00:05:00",
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

Distributed backend registrations accept either a `Func<TOptions, TOptions>` transform or an `IConfigurationSection`,
validate their options at startup, and can be called before or after `AddDistributedMode`:

```csharp
builder.AddRedisDistributedCoordinator(builder.Configuration.GetSection("Redis"));
builder.AddSignalRDistributedCoordinator(builder.Configuration.GetSection("SignalR"));
builder.AddRedisMasterDiscovery(builder.Configuration.GetSection("RedisDiscovery"));
builder.AddS3DistributedArtifactStore(builder.Configuration.GetSection("S3"));
```

`AddRedisMasterDiscovery` keys the advertised master endpoint by `DistributedOptions.RunId` and enables
`RequireExplicitRunId`, so every process in the run must share one explicit `RunId`: set it in the
`Distributed` section, return `options with { RunId = "shared-run-id" }` from the configuration callback, or export `MODULARPIPELINES_RUN_ID` for each master and
worker. See [Run Identifier Resolution](#run-identifier-resolution).

`AddRedisModuleCache` and `AddS3ModuleCache` each have four overloads. For `TOptions` equal to `RedisOptions`
or `S3StorageOptions`, respectively, the arguments after the builder are:

| Arguments | Configuration |
|-----------|---------------|
| `Func<TOptions, TOptions> configure` | Backend options transform. |
| `Func<TOptions, TOptions> configure, Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache` | Backend and module-cache options transforms. |
| `IConfigurationSection section` | Backend options bound from configuration. |
| `IConfigurationSection section, Func<ModuleCacheOptions, ModuleCacheOptions>? configureCache` | Backend options binding and a module-cache options transform. |

The overloads without `configureCache` use the default module-cache settings. Module-cache registration does not
enable distributed execution; see [Cache Module Results](../how-to/module-caching.md).

Only one coordinator backend and one artifact store backend can be registered;
registering a second, different backend throws `InvalidOperationException` instead of silently replacing
the first. Registering the same backend again is a no-op apart from applying the extra configuration.

The same conflict rule applies to `AddDistributedArtifactStore<TStore>()` and
`AddDistributedArtifactStoreFactory<TFactory>()`: do not mix a direct store and a factory,
regardless of registration order. Repeating the same typed registration is a no-op. Keyed
services are independent, and pipelines without an explicit store retain the default filesystem store.
In V4, direct store registration no longer silently removes an earlier factory; choose one backend
at the registration call site instead.

## Artifact compression

`DistributedOptions.ArtifactCompressionLevel` applies to every artifact store, including in local mode. Configure it with the
options pattern, independently of the backend:

```csharp
builder.ConfigureDistributedOptions(o => o with { ArtifactCompressionLevel = CompressionLevel.Optimal });
// Or return updated options from your existing AddDistributedMode configuration:
builder.AddDistributedMode(o => o with { ArtifactCompressionLevel = CompressionLevel.Optimal });
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ArtifactCompressionLevel` | `CompressionLevel` | `Fastest` | Compression level for directory and glob artifacts. |

Storage-specific settings such as expiry, chunk size and multipart part size live on the backend's options
(`RedisOptions`, `S3StorageOptions`).

## Run data cleanup

When a distributed pipeline ends, backends that implement `IDistributedRunCleanup` delete the data they stored
for the run. Only the master cleans up. Workers never do, and neither do pipelines that do not call
`AddDistributedMode`, because separate local runs can share a run identifier and its artifacts. Cleanup starts
after the pipeline end handlers and the run report have finished, and only once every worker registered for the
run has published its final status. Workers publish it after their own pipeline end handlers, so none still uses
the run's data. If a worker does not publish one within `RunCleanupTimeout`, for example because it crashed,
cleanup is skipped with a warning and each backend's own expiry removes the data.

`DistributedOptions.RunCleanup` selects when cleanup runs:

| Value | Behavior |
|-------|----------|
| `Always` (default) | Deletes the run's data whatever the outcome. |
| `OnSuccess` | Deletes the run's data when the pipeline succeeded. A failed or canceled run keeps its data for investigation until each backend's own expiry removes it. |
| `Never` | Keeps the run's data; only each backend's own expiry removes it. |

```csharp
builder.AddDistributedMode(o => o with
{
    RunCleanup = DistributedRunCleanupPolicy.OnSuccess,
    RunCleanupTimeout = TimeSpan.FromMinutes(5),
});
```

Cleanup is best effort. A backend that fails, or does not finish within `RunCleanupTimeout`, is logged as a
warning and never changes the pipeline result. Backend expiry, such as `RedisOptions.TimeToLive` or the S3
lifecycle rule, still removes data from runs that crashed or kept their data. Module caches are shared across
runs, so cleanup never deletes them.

The built-in backends clean up as follows:

| Backend | Deletes |
|---------|---------|
| Redis coordinator | Every run coordination key except the completion flag, which expires with `TimeToLive` so a worker that starts late sees a completed master and exits. |
| Redis artifact store | Every artifact uploaded in the run, by any process, and the indexes that list them. |
| S3 artifact store | Every object under `{KeyPrefix}/artifacts/{RunId}/`. In a versioned bucket it deletes every version and delete marker, so no artifact stays recoverable. Providers without a versioning API, such as Cloudflare R2, fall back to deleting the current objects. |
| Redis master discovery | The endpoint and access token this master advertised. |

To make a custom coordinator, artifact store or discovery service clean up, implement `IDistributedRunCleanup` on it.
Any other service can take part by registering itself as `IDistributedRunCleanup`. Each backend is called once,
even when it is registered more than once. See the [architecture guide](architecture.md#run-data-cleanup) for the
contract.

## RedisOptions

The Redis coordinator requires Redis 7.4 or later, or Valkey 9.0 or later. Worker heartbeats use hash field
expiry (`HPEXPIRE`), and notifications use sharded pub/sub (`SPUBLISH`), which keeps a run's messages on the
shard that owns its keys in Redis Cluster. An older server fails when the coordinator is created.

Passed to `AddRedisDistributedCoordinator()`, `AddRedisDistributedArtifactStore()`, `AddRedisDistributed()` and
`AddRedisModuleCache()`. Controls how the Redis features connect and manage keys. The coordinator and artifact
store share one `RedisOptions` instance but open separate connections, so large artifact transfers do not queue
ahead of coordinator heartbeats on the client. Both connections still share the network link and the Redis server,
so size them for the combined load. The module cache has its own options and connection, so it can use a different Redis server.

```csharp
builder.AddRedisDistributed(o => o with
{
    ConnectionString = "redis-host:6379",
    ConfigureConnection = connection => connection with
    {
        Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD"),
        UseTls = true,
    },
    KeyPrefix = "modpipe",
    TimeToLive = TimeSpan.FromHours(1),
});
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ConnectionString` | `string` | `""` | [Respire connection string](https://respire.tomlonghurst.dev/docs/fundamentals/connections.md): `host:port`, a `redis://` or `rediss://` URI, or a StackExchange.Redis-style comma-delimited string (`password`, `ssl`, `asyncTimeout`, etc.). Unknown options such as `abortConnect` fail validation. **Required** unless `ConfigureConnection` supplies the endpoints. |
| `ConfigureConnection` | `Func<RespireOptions, RespireOptions>?` | `null` | Transforms the parsed `RespireOptions` before connecting, usually with a `with` expression. Use it for passwords containing commas or other characters a connection string cannot carry, TLS, timeouts, and retry settings. |
| `KeyPrefix` | `string` | `"modpipe"` | Prefix for all Redis keys. Change this if multiple different pipelines share the same Redis instance. |
| `TimeToLive` | `TimeSpan` | `TimeSpan.FromHours(1)` | TTL for all Redis keys, including artifacts and module cache entries. Keys are automatically cleaned up after this duration, and [run data cleanup](#run-data-cleanup) deletes a finished run's keys earlier. |
| `ChunkSizeBytes` | `int` | 1 MB | Size of each Redis value used for artifacts and module cache entries. Content that fits in one chunk is stored under a single key. Each chunk must transfer within the connection's `CommandTimeout`, so lower it or raise the timeout for slow or shared links. |
| `MasterStartTimeout` | `TimeSpan` | `TimeSpan.FromMinutes(5)` | Positive, finite deadline from worker registration for the master to start. Must be at least `DistributedOptions.WorkerHeartbeatInterval`. Expiry fails the worker as a lost master. Reconnects do not reset it; after startup, `DistributedOptions.MasterTimeout` governs heartbeat loss. |
| `FailOnUnsafeMemoryPolicy` | `bool` | `false` | Fail coordinator and artifact-store startup with `InvalidOperationException` when `INFO memory` reports finite `maxmemory` with an evicting policy, or diagnostics cannot establish the configuration. The default logs a warning and continues. Module-cache connections are not checked. |

Use `noeviction` or unlimited `maxmemory` to prevent run state and artifact chunks from being evicted before
their TTL expires. The startup memory check needs `INFO` permission and observes only the server answering
the command, not every Redis Cluster node. It does not guard against later configuration changes.

Each registration owns its connection and connects asynchronously on first use. The package never registers or
resolves an `IRespireClient` from the service collection, so an application's own client is not adopted and
cannot conflict with it.

Unless the connection string or `ConfigureConnection` sets one, the connection uses Respire's default
`RespireRetryPolicy`. After a dropped connection, Respire fails the next command while it reconnects; the policy
resends commands that were never written, and reads, so a brief disconnect does not fail coordination. Set
`RetryPolicy = null` in `ConfigureConnection` to turn it off.

The options are validated when the pipeline is built. After configuration and post-configuration finish,
`ConfigureConnection` runs once per materialized options instance. The effective configuration must have
at least one endpoint: a no-op or TLS-only callback without a connection string fails, as does a callback
that removes every parsed endpoint. A connection string that Respire cannot parse also fails. Validation does
not connect to Redis. Connections and retries reuse copies of the validated snapshot; later option changes or
changes to an endpoint list the callback kept do not change that snapshot. Named module-cache options have
their own snapshots.

A missing connection, an empty `KeyPrefix`, or a
non-positive `TimeToLive`, `MasterStartTimeout` or `ChunkSizeBytes` fails fast with `OptionsValidationException`. For the coordinator
and artifact store, `MasterStartTimeout` must be at least `DistributedOptions.WorkerHeartbeatInterval`,
so polling cannot take longer than the startup timeout. For the coordinator
and artifact store, `TimeToLive` must also exceed `DistributedOptions.ModuleResultTimeout` so run keys and
artifacts cannot expire mid-run, and `DistributedOptions.MasterTimeout` so workers see a stopped master's
heartbeat go stale before it expires. An infinite module result timeout does not disable Redis key
expiration: keys retain their configured TTL, so choose a TTL that covers the required retention period.
Coordinator creation additionally requires `TimeToLive` to exceed `DistributedOptions.WorkerTimeout`.
Named module-cache options require a positive TTL but are not compared with these distributed timeouts.

All distributed duration properties use `TimeSpan`. When binding them from `appsettings.json`, use the invariant `TimeSpan` string format:

```json
{
  "Distributed": {
    "WorkerRegistrationTimeout": "00:05:00",
    "WorkerHeartbeatInterval": "00:00:05",
    "WorkerTimeout": "00:00:30",
    "MasterTimeout": "00:01:00",
    "ModuleResultTimeout": "00:45:00"
  },
  "Redis": {
    "TimeToLive": "01:00:00"
  },
  "SignalR": {
    "ConnectionTimeout": "00:02:00",
    "KeepAliveInterval": "00:00:05",
    "PeerTimeout": "00:00:15",
    "Tunnel": {
      "StartupTimeout": "00:00:30"
    }
  },
  "RedisDiscovery": {
    "TimeToLive": "01:00:00",
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

All coordination keys follow the pattern `{KeyPrefix}:{{RunId}}:{purpose}`. The braces around the run
identifier form a Redis Cluster hash tag, so every key of one run lives in one slot and the Lua scripts can
touch several keys atomically. With the defaults, keys look like:

| Key | Redis Type | Purpose |
|-----|-----------|---------|
| `modpipe:{run}:work:queue` | Sorted set | Queued assignments, scored by priority and critical-path weight |
| `modpipe:{run}:work:leases` | Hash | Active leases (field = module ID; the value holds the lease ID, worker ID and expiry) |
| `modpipe:{run}:results` | Hash | Final module results (field = module ID), written with `HSETNX` after lease validation so the first accepted result wins |
| `modpipe:{run}:workers` | Hash | Worker registrations (field = worker ID) and `heartbeat:{worker ID}` timestamps |
| `modpipe:{run}:workers:status` | Hash | Latest `WorkerStatus` per worker |
| `modpipe:{run}:completion` | String | Set when the master signals completion |
| `modpipe:{run}:cancellation` | String | Cancellation reason (`PipelineFailed` or `Stopped`) |

Pub/Sub channels (no TTL, ephemeral):

| Channel | Purpose |
|---------|---------|
| `modpipe:{run}:work:available` | Wakes workers waiting to claim work |
| `modpipe:{run}:results:{ModuleId}` | Wakes waiters for a module's result |
| `modpipe:{run}:completion:signal` | Wakes workers when the master signals completion |
| `modpipe:{run}:cancellation:signal` | Wakes cancellation observers |

Notifications use sharded pub/sub (`SPUBLISH`) and only shorten waits: every wait also re-reads Redis every two
seconds, so a message lost while the connection reconnects cannot strand a waiter. Heartbeats and the master's
lease sweep refresh coordination key expiry. Unnamed `RedisOptions.TimeToLive` must exceed `ModuleResultTimeout`
and `MasterTimeout` during options validation; coordinator creation also requires it to exceed `WorkerTimeout`.
Named module-cache options are independent of these run timeouts. Only the master renews its own heartbeat, while a
marker that the master started is renewed with the other run keys, so a master whose heartbeat went stale or
expired is reported as lost rather than as not started yet.

Artifacts are stored under the same run namespace:

| Key | Redis Type | Purpose |
|-----|-----------|---------|
| `modpipe:{run}:artifacts:meta:{id}` | String | Artifact metadata (JSON) |
| `modpipe:{run}:artifacts:data:{id}` | String | Content of an artifact that fits in one chunk (including empty artifacts) |
| `modpipe:{run}:artifacts:data:{id}:chunk:{n}` | String | Chunks of a larger artifact |
| `modpipe:{run}:artifacts:index:{ModuleId}` | Set | Artifact ids uploaded by a module |
| `modpipe:{run}:artifacts:modules` | Set | Modules that uploaded artifacts in the run, used by run data cleanup |

The braces around the run identifier form a Redis Cluster hash tag, so every key of a run maps to one slot.
Module cache entries are not run-scoped; they use `{KeyPrefix}:module-cache:v2:{fingerprint}:...`, where the
braced fingerprint is the hash tag that keeps an entry's metadata and chunks in one slot.

All storage keys have the configured TTL applied, so they are automatically cleaned up even if the pipeline crashes.
[Run data cleanup](#run-data-cleanup) deletes a finished run's keys before the TTL expires.

## SignalR Coordinator

`AddSignalRDistributedCoordinator` makes the master host a SignalR hub. Workers connect to it and pull
leases, publish results and observe cancellation through hub invocations, so a worker that reconnects
invokes again and never misses a broadcast.

```csharp
builder.AddSignalRDistributedCoordinator(options => options with
{
    ListenUrl = new Uri("https://0.0.0.0:5099"),
    AdvertisedUrl = new Uri("https://pipeline-master.example.com"),
    AccessToken = Environment.GetEnvironmentVariable("PIPELINE_HUB_TOKEN"),
});
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ListenUrl` | `Uri` | `http://localhost:5099` | HTTPS URL the master binds (HTTP only on loopback). Port `0` lets the operating system choose a port. |
| `AdvertisedUrl` | `Uri?` | `null` | HTTPS URL workers connect to (HTTP only on loopback). The master advertises it through discovery; workers without discovery connect to it, or to `ListenUrl` when it is unset. |
| `HubPath` | `string` | `/pipeline-hub` | Hub path, starting with `/`. |
| `AccessToken` | `string?` | `null` | Token workers present to the master. When unset, the master generates one whenever it is reachable beyond the machine (a tunnel or a non-loopback `ListenUrl`) and shares it through master discovery; without discovery such a master fails at startup until every process configures a token. The token is never logged. |
| `ConnectionTimeout` | `TimeSpan` | `00:02:00` | How long a worker keeps trying to connect. |
| `MaxReconnectAttempts` | `int` | `5` | Maximum automatic reconnect attempts with exponential backoff; `0` disables reconnecting. Recovery, including attempts and re-registration, is bounded by `WorkerTimeout - WorkerHeartbeatInterval - PeerTimeout` (zero when no budget remains). |
| `KeepAliveInterval` | `TimeSpan` | `00:00:05` | Ping interval on both sides. |
| `PeerTimeout` | `TimeSpan` | `00:00:15` | Silence after which a side considers its peer gone; at least twice `KeepAliveInterval`. |
| `MaxMessageSizeBytes` | `long` | `1048576` | Largest SignalR message, such as a module result. |
| `Tunnel.Enabled` | `bool` | `false` | Start a cloudflared quick tunnel to the bound URL and advertise its public URL. |
| `Tunnel.CloudflaredPath` | `string` | `cloudflared` | Path to the cloudflared binary. |
| `Tunnel.StartupTimeout` | `TimeSpan` | `00:00:30` | How long to wait for the tunnel URL. |

SignalR requires HTTPS for every non-loopback endpoint, whether configured directly or returned by
master discovery, even when no access token is configured. Configure Kestrel's server certificate on
the master, for example with `Kestrel__Certificates__Default__Path` and
`Kestrel__Certificates__Default__Password`, and ensure workers trust its certificate. For a reverse
proxy or cloudflared tunnel that terminates HTTPS, the local listener can use
`http://127.0.0.1:5099` while workers connect to the public HTTPS URL.

The hub rejects registrations for another `RunId`, a second live worker with the same `WorkerId`, and every
call from a connection that has not registered; results and heartbeats must come from the registered worker.
Detailed hub errors are disabled, and the tunnel URL is logged only at Debug.

## Redis Master Discovery

`AddRedisMasterDiscovery` stores the SignalR master's `MasterEndpoint`, its URL and access token, in Redis
so workers can find it. Discovery opens its own lazily connected [Respire](https://respire.tomlonghurst.dev/)
connection; it neither uses nor replaces a Redis client registered by the application.

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ConnectionString` | `string` | Empty | Explicit Respire connection string; required unless `ConfigureConnection` supplies endpoints or both REST options are set. Unknown options such as `abortConnect` fail validation. |
| `ConfigureConnection` | `Func<RespireOptions, RespireOptions>?` | `null` | Transforms the TCP configuration during validation, for example credentials or TLS; must leave at least one endpoint. The configuration is reused when connecting. Defaults to Respire's `RespireRetryPolicy` unless one is set. Ignored for REST. |
| `RestUrl` | `Uri?` | `null` | Absolute HTTPS Upstash REST endpoint (HTTP only on loopback); configure together with `RestToken`. |
| `RestToken` | `string?` | `null` | Upstash REST token; configure together with `RestUrl`. |
| `KeyPrefix` | `string` | `modpipe` | Prefix of the endpoint key `{KeyPrefix}:{RunId}:master-endpoint`. |
| `TimeToLive` | `TimeSpan` | `01:00:00` | Expiry of the advertised endpoint, renewed while the master is active. Must be at least one second. [Run data cleanup](#run-data-cleanup) deletes it earlier. |
| `DiscoveryTimeout` | `TimeSpan` | `00:02:00` | How long workers wait for the endpoint. |
| `PollInterval` | `TimeSpan` | `00:00:00.500` | How often workers check for the endpoint. |

The master renews its advertisement every third of `TimeToLive`, with at most one minute between renewals,
so workers can discover it even after the original expiry during a long run. Renewal and cleanup atomically
check ownership: an expired, removed, or replacement advertisement is never recreated or modified by an old
master. Redis credentials need `GET`, `SET`, `EVALSHA`, `EVAL`, `PEXPIRE`, and `DEL` permission (including the commands
called by the scripts). Both TCP and REST discovery support renewal.

[Run data cleanup](#run-data-cleanup) stops renewal before deleting this master's endpoint. Service disposal
also stops renewal when cleanup is skipped; in that case, or after a process crash, the last advertisement
expires within `TimeToLive`. Renewal failures log a warning without the endpoint value or access token and
retry at the next interval. If Redis remains unavailable until expiry, discovery is lost and the master must
be restarted with a fresh run identifier; renewal does not recreate expired advertisements.

Because the stored endpoint includes the access token, only the run's processes should be able to read the
discovery Redis database.

## Connection String Examples

The Redis coordinator, artifact store, module cache and master discovery parse connection strings with Respire,
which accepts StackExchange.Redis-style strings but rejects options it does not support, such as `abortConnect`.

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
redis-host:6380,password=mysecret,ssl=true
```

**Multiple endpoints (Redis Cluster):**
```
host1:6379,host2:6379,password=mysecret,cluster=true
```

Respire requires `cluster=true` (or `serviceName` for Sentinel) when a connection string lists more than one
endpoint. See the [Respire connection docs](https://respire.tomlonghurst.dev/docs/fundamentals/connections.md)
for all connection string options.

## V4 endpoint migration

`SignalRDistributedOptions.ListenUrl`, `S3StorageOptions.ServiceUrl`, and
`RedisDiscoveryOptions.RestUrl` now use `Uri` instead of `string`, matching
`SignalRDistributedOptions.AdvertisedUrl`. In C# configuration callbacks, wrap
endpoint strings in `new Uri("https://host.example")`. Endpoints must be absolute
HTTP or HTTPS URLs; relative URLs and other schemes fail options validation.
Redis REST endpoints require HTTPS except on loopback because requests include a bearer token.
They may include a path prefix, but must not contain a query string or fragment.
SignalR listener, advertised and discovered endpoints also require HTTPS except on loopback, to
protect access tokens and module payloads. Non-loopback HTTP URLs now fail validation or worker startup.
Empty or whitespace optional S3/Redis endpoints are treated as omitted.

`ListenUrl` represents one endpoint, not a semicolon-separated list of Kestrel
addresses. Kestrel wildcard hosts such as `https://+:5099` and `https://*:5099` are
not valid `Uri` values. To listen on all IPv4 interfaces, use
`new Uri("https://0.0.0.0:5099")` and set `AdvertisedUrl` to an HTTPS address workers can reach.

Configuration files and environment variables still contain URL strings; normal
configuration binding converts them to `Uri`. SignalR retains its
`http://localhost:5099` default and supports port `0` for an assigned port. S3's
service endpoint and Redis's REST endpoint remain optional: `null` selects the AWS
region endpoint or Redis TCP connection, respectively. Configure a Redis REST
token together with its REST endpoint.

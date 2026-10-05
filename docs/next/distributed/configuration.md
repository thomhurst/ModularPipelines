# Configuration

Distributed mode has two layers of configuration: the core `DistributedOptions` (shared across all coordinator implementations) and backend-specific options like `RedisOptions`.

## Module identity and build compatibility[​](#module-identity-and-build-compatibility "Direct link to Module identity and build compatibility")

Assignments, stored results, dependency references, artifact descriptors, and worker command counts use `ModuleId`. The value is case-sensitive and serializes as a JSON string. By default it derives from the module's full type name; generic arguments omit assembly versions. Assign an explicit ID to preserve identity when renaming or moving a module:

```
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

## DistributedOptions[​](#distributedoptions "Direct link to DistributedOptions")

Passed to `AddDistributedMode()`. Controls the fundamental behavior of the master/worker system.

```
builder.AddDistributedMode(o =>

{

    o.InstanceIndex = 0;

    o.TotalInstances = 4;

    o.MaxParallelism = 4;

    o.Role = DistributedRole.Master;

    o.Capabilities = [Capability.Docker, Capability.Gpu];

    o.RunId = Environment.GetEnvironmentVariable("MODULARPIPELINES_RUN_ID")!;

    o.WorkerRegistrationTimeout = TimeSpan.FromMinutes(5);

    o.WorkerHeartbeatInterval = TimeSpan.FromSeconds(5);

    o.WorkerTimeout = TimeSpan.FromSeconds(30);

    o.MasterTimeout = TimeSpan.FromMinutes(1);

    o.MinimumWorkerCount = 0;

    o.ModuleResultTimeout = TimeSpan.FromMinutes(45);

});
```

| Property                    | Type                        | Default                                                 | Description                                                                                                                                                                                                                                                                                                                                                                      |
| --------------------------- | --------------------------- | ------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Role`                      | `DistributedRole`           | `Auto`                                                  | Explicit `Master` or `Worker` role. `Auto` derives the role from `InstanceIndex`.                                                                                                                                                                                                                                                                                                |
| `InstanceIndex`             | `int`                       | `0`                                                     | This instance's unique index, less than `TotalInstances`. With `Role == Auto`, `0` selects master and values above `0` select worker. Each process's `WorkerId` is `instance-{InstanceIndex}`.                                                                                                                                                                                   |
| `TotalInstances`            | `int`                       | `1`                                                     | Total number of instances (master + workers). Values above `1` require a shared coordinator backend.                                                                                                                                                                                                                                                                             |
| `MaxParallelism`            | `int?`                      | `null`                                                  | Optional per-node concurrency limit. It can lower, but cannot raise, the pipeline's global `Concurrency.MaxParallelism` limit. Workers advertise it when they register.                                                                                                                                                                                                          |
| `Capabilities`              | `IReadOnlyList<Capability>` | `[]`                                                    | Capabilities this instance advertises in addition to those detected by registered `ICapabilityProvider` services, including the current OS. Built-in values are available from `Capability`; create custom values with `new Capability("name")`.                                                                                                                                 |
| `RunId`                     | `string`                    | `MODULARPIPELINES_RUN_ID` or generated for one instance | Identifier shared by every process in this pipeline run: 1-128 ASCII letters, digits, `.`, `_` or `-`, because backends embed it in keys and URLs. Multi-instance runs fail fast when neither source is configured.                                                                                                                                                              |
| `RequireExplicitRunId`      | `bool`                      | `false`                                                 | Reject generated single-instance IDs. The Redis and SignalR coordinators and Redis discovery enable this automatically. S3 artifact-store registrations also require an explicit shared `RunId`, including for single-instance configurations. Custom backends opt in with `builder.RequireExplicitRunId()`.                                                                     |
| `WorkerRegistrationTimeout` | `TimeSpan`                  | `TimeSpan.FromMinutes(5)`                               | How long the master waits for workers to register: for `MinimumWorkerCount` before dispatch starts, and for a capable worker before an assignment the master cannot run fails with a routing error.                                                                                                                                                                              |
| `WorkerHeartbeatInterval`   | `TimeSpan`                  | `TimeSpan.FromSeconds(5)`                               | How often workers, and the master's own worker loop, report liveness and renew the leases on their in-flight modules.                                                                                                                                                                                                                                                            |
| `WorkerTimeout`             | `TimeSpan`                  | `TimeSpan.FromSeconds(30)`                              | How long a registration and its leases stay valid without a heartbeat. When a lease expires the master returns its module to the queue. Must exceed `WorkerHeartbeatInterval`.                                                                                                                                                                                                   |
| `MasterTimeout`             | `TimeSpan`                  | `TimeSpan.FromMinutes(1)`                               | How long workers keep running after the master stops sending heartbeats without having signalled completion, or while they cannot reach the coordinator to check. Workers then cancel their in-flight modules, including AlwaysRun modules, and fail. SignalR workers detect a lost master when their connection closes for good instead. Must exceed `WorkerHeartbeatInterval`. |
| `MinimumWorkerCount`        | `int`                       | `0`                                                     | Number of external workers required before dispatch starts, between zero and `TotalInstances - 1`. Keep zero for immediate dispatch.                                                                                                                                                                                                                                             |
| `ModuleResultTimeout`       | `TimeSpan`                  | `TimeSpan.FromMinutes(45)`                              | The master's backstop for a claimed module. The deadline starts when a worker claims the module and allows every configured attempt at the module's timeout plus this period; the worker enforces the per-attempt timeout itself. Use `TimeSpan.Zero` to wait indefinitely.                                                                                                      |

Invalid combinations, such as `InstanceIndex >= TotalInstances`, `WorkerTimeout <= WorkerHeartbeatInterval` or `MasterTimeout <= WorkerHeartbeatInterval`, are reported by pipeline validation and stop the distributed backend from starting.

### Configuration from appsettings.json[​](#configuration-from-appsettingsjson "Direct link to Configuration from appsettings.json")

You can also bind from configuration:

```
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

```
builder.AddDistributedMode(builder.Configuration.GetSection("Distributed"));
```

Or call `builder.AddDistributedMode()` to bind the standard environment variables:

| Environment variable               | Option                                 |
| ---------------------------------- | -------------------------------------- |
| `MODULARPIPELINES_INSTANCE_INDEX`  | `InstanceIndex`                        |
| `MODULARPIPELINES_TOTAL_INSTANCES` | `TotalInstances`                       |
| `MODULARPIPELINES_MAX_PARALLELISM` | `MaxParallelism`                       |
| `MODULARPIPELINES_RUN_ID`          | `RunId`                                |
| `MODULARPIPELINES_ROLE`            | `Role` (`Auto`, `Master`, or `Worker`) |

Configuration binding converts the string array to `Capability` values. Distributed wire payloads also remain plain JSON strings.

Every satellite package registers through one `Action<TOptions>` overload and one `IConfigurationSection` overload, validates its options at startup, and can be called before or after `AddDistributedMode`:

```
builder.AddRedisDistributedCoordinator(builder.Configuration.GetSection("Redis"));

builder.AddSignalRDistributedCoordinator(builder.Configuration.GetSection("SignalR"));

builder.AddRedisMasterDiscovery(builder.Configuration.GetSection("RedisDiscovery"));

builder.AddS3DistributedArtifactStore(builder.Configuration.GetSection("S3"));
```

`AddRedisMasterDiscovery` keys the advertised master endpoint by `DistributedOptions.RunId` and enables `RequireExplicitRunId`, so every process in the run must share one explicit `RunId`: set it in the `Distributed` section, assign `options.RunId`, or export `MODULARPIPELINES_RUN_ID` for each master and worker. See [Run Identifier Resolution](#run-identifier-resolution).

Each backend registration has exactly two overloads: one taking an `Action<TOptions>` and one taking an `IConfigurationSection`. Only one coordinator backend and one artifact store backend can be registered; registering a second, different backend throws `InvalidOperationException` instead of silently replacing the first. Registering the same backend again is a no-op apart from applying the extra configuration.

The same conflict rule applies to `AddDistributedArtifactStore<TStore>()` and `AddDistributedArtifactStoreFactory<TFactory>()`: do not mix a direct store and a factory, regardless of registration order. Repeating the same typed registration is a no-op. Keyed services are independent, and pipelines without an explicit store retain the default filesystem store. In V4, direct store registration no longer silently removes an earlier factory; choose one backend at the registration call site instead.

## ArtifactOptions[​](#artifactoptions "Direct link to ArtifactOptions")

`ArtifactOptions` holds the artifact settings that apply to every artifact store. Configure them once with the options pattern, independently of the backend:

```
builder.Services.Configure<ArtifactOptions>(o => o.CompressionLevel = CompressionLevel.Optimal);

// or: builder.Services.Configure<ArtifactOptions>(builder.Configuration.GetSection("Artifacts"));
```

| Property           | Type               | Default   | Description                                |
| ------------------ | ------------------ | --------- | ------------------------------------------ |
| `CompressionLevel` | `CompressionLevel` | `Fastest` | Compression level for directory artifacts. |

Storage-specific settings such as expiry, chunk size and multipart part size live on the backend's options (`RedisOptions`, `S3StorageOptions`).

## RedisOptions[​](#redisoptions "Direct link to RedisOptions")

Passed to `AddRedisDistributedCoordinator()`, `AddRedisDistributedArtifactStore()`, `AddRedisDistributed()` and `AddRedisModuleCache()`. Controls how the Redis features connect and manage keys. The coordinator and artifact store share one `RedisOptions` instance but open separate connections, so large artifact transfers do not queue ahead of coordinator heartbeats on the client. Both connections still share the network link and the Redis server, so size them for the combined load. The module cache has its own options and connection, so it can use a different Redis server.

```
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

| Property              | Type                            | Default                 | Description                                                                                                                                                                                                                                                    |
| --------------------- | ------------------------------- | ----------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `ConnectionString`    | `string`                        | `""`                    | StackExchange.Redis connection string. Supports all standard options (`password`, `ssl`, `abortConnect`, etc.). **Required** unless `ConfigureConnection` supplies the endpoints.                                                                              |
| `ConfigureConnection` | `Action<ConfigurationOptions>?` | `null`                  | Adjusts the parsed `ConfigurationOptions` before connecting. Use it for passwords containing commas or other characters a connection string cannot carry, TLS, and retry settings.                                                                             |
| `KeyPrefix`           | `string`                        | `"modpipe"`             | Prefix for all Redis keys. Change this if multiple different pipelines share the same Redis instance.                                                                                                                                                          |
| `TimeToLive`          | `TimeSpan`                      | `TimeSpan.FromHours(1)` | TTL for all Redis keys, including artifacts and module cache entries. Keys are automatically cleaned up after this duration.                                                                                                                                   |
| `ChunkSizeBytes`      | `int`                           | 1 MB                    | Size of each Redis value used for artifacts and module cache entries. Content that fits in one chunk is stored under a single key. Each chunk must transfer within the connection's `AsyncTimeout`, so lower it or raise the timeout for slow or shared links. |

Each registration owns its connection and connects asynchronously on first use. The package never registers or resolves an `IConnectionMultiplexer` from the service collection, so an application's own multiplexer is not adopted and cannot conflict with it.

The options are validated when the pipeline is built. After configuration and post-configuration finish, `ConfigureConnection` runs once per materialized options instance. The effective configuration must have at least one endpoint: a no-op or TLS-only callback without a connection string fails, as does a callback that removes every parsed endpoint. Validation does not connect to Redis. Connections and retries reuse copies of the validated snapshot; later option changes or mutations of a retained callback argument do not change that snapshot. Named module-cache options have their own snapshots.

A missing connection, an empty `KeyPrefix`, or a non-positive `TimeToLive` or `ChunkSizeBytes` fails fast with `OptionsValidationException`. For the coordinator and artifact store, `TimeToLive` must also exceed `DistributedOptions.ModuleResultTimeout` so run keys and artifacts cannot expire mid-run, and `DistributedOptions.MasterTimeout` so workers see a stopped master's heartbeat go stale before it expires.

All distributed duration properties use `TimeSpan`. When binding them from `appsettings.json`, use the invariant `TimeSpan` string format:

```
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

## Run Identifier Resolution[​](#run-identifier-resolution "Direct link to Run Identifier Resolution")

Distributed coordination requires an invocation-scoped identifier. It is resolved in this order:

| Priority | Source                            | Environment                     |
| -------- | --------------------------------- | ------------------------------- |
| 1        | `DistributedOptions.RunId`        | Explicit configuration          |
| 2        | `MODULARPIPELINES_RUN_ID` env var | Any CI or local orchestration   |
| 3        | Generated GUID                    | Single-process/default fallback |

Use an invocation-specific value rather than a stable commit identifier so rerunning the same commit receives a fresh Redis namespace. Local multi-process runs should export one unique `MODULARPIPELINES_RUN_ID` value before starting the master and workers. CI workflows must likewise generate or derive one invocation-specific value and export it as `MODULARPIPELINES_RUN_ID` for every master and worker.

## Redis Key Schema[​](#redis-key-schema "Direct link to Redis Key Schema")

All coordination keys follow the pattern `{KeyPrefix}:{{RunId}}:{purpose}`. The braces around the run identifier form a Redis Cluster hash tag, so every key of one run lives in one slot and the Lua scripts can touch several keys atomically. With the defaults, keys look like:

| Key                            | Redis Type | Purpose                                                                                  |
| ------------------------------ | ---------- | ---------------------------------------------------------------------------------------- |
| `modpipe:{run}:work:queue`     | Sorted set | Queued assignments, scored by priority and critical-path weight                          |
| `modpipe:{run}:work:leases`    | Hash       | Active leases (field = module ID; the value holds the lease ID, worker ID and expiry)    |
| `modpipe:{run}:results`        | Hash       | Final module results (field = module ID), written with `HSETNX` so the first result wins |
| `modpipe:{run}:workers`        | Hash       | Worker registrations (field = worker ID) and `heartbeat:{worker ID}` timestamps          |
| `modpipe:{run}:workers:status` | Hash       | Latest `WorkerStatus` per worker                                                         |
| `modpipe:{run}:completion`     | String     | Set when the master signals completion                                                   |
| `modpipe:{run}:cancellation`   | String     | Cancellation reason (`PipelineFailed` or `Stopped`)                                      |

Pub/Sub channels (no TTL, ephemeral):

| Channel                             | Purpose                                          |
| ----------------------------------- | ------------------------------------------------ |
| `modpipe:{run}:work:available`      | Wakes workers waiting to claim work              |
| `modpipe:{run}:results:{ModuleId}`  | Wakes waiters for a module's result              |
| `modpipe:{run}:completion:signal`   | Wakes workers when the master signals completion |
| `modpipe:{run}:cancellation:signal` | Wakes cancellation observers                     |

Notifications only shorten waits: every wait also re-reads Redis every two seconds, so a message lost while the connection reconnects cannot strand a waiter. Heartbeats and the master's lease sweep refresh the expiry of every coordination key, and `RedisOptions.TimeToLive` must exceed `WorkerTimeout`, `ModuleResultTimeout` and `MasterTimeout`, so keys cannot expire while a run is active. Only the master renews its own heartbeat, while a marker that the master started is renewed with the other run keys, so a master whose heartbeat went stale or expired is reported as lost rather than as not started yet.

Artifacts are stored under the same run namespace:

| Key                                           | Redis Type | Purpose                                                                   |
| --------------------------------------------- | ---------- | ------------------------------------------------------------------------- |
| `modpipe:{run}:artifacts:meta:{id}`           | String     | Artifact metadata (JSON)                                                  |
| `modpipe:{run}:artifacts:data:{id}`           | String     | Content of an artifact that fits in one chunk (including empty artifacts) |
| `modpipe:{run}:artifacts:data:{id}:chunk:{n}` | String     | Chunks of a larger artifact                                               |
| `modpipe:{run}:artifacts:index:{ModuleId}`    | Set        | Artifact ids uploaded by a module                                         |

The braces around the run identifier form a Redis Cluster hash tag, so every key of a run maps to one slot. Module cache entries are not run-scoped; they use `{KeyPrefix}:module-cache:v2:{fingerprint}:...`, where the braced fingerprint is the hash tag that keeps an entry's metadata and chunks in one slot.

All storage keys have the configured TTL applied, so they are automatically cleaned up even if the pipeline crashes.

## SignalR Coordinator[​](#signalr-coordinator "Direct link to SignalR Coordinator")

`AddSignalRDistributedCoordinator` makes the master host a SignalR hub. Workers connect to it and pull leases, publish results and observe cancellation through hub invocations, so a worker that reconnects invokes again and never misses a broadcast.

```
builder.AddSignalRDistributedCoordinator(options =>

{

    options.ListenUrl = "http://0.0.0.0:5099";

    options.AdvertisedUrl = new Uri("https://pipeline-master.example.com");

    options.AccessToken = Environment.GetEnvironmentVariable("PIPELINE_HUB_TOKEN");

});
```

| Property                 | Type       | Default                 | Description                                                                                                                                                                                                                                                                                                                |
| ------------------------ | ---------- | ----------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `ListenUrl`              | `string`   | `http://localhost:5099` | URL the master binds. Port `0` lets the operating system choose a port.                                                                                                                                                                                                                                                    |
| `AdvertisedUrl`          | `Uri?`     | `null`                  | URL workers connect to. The master advertises it through discovery; workers without discovery connect to it, or to `ListenUrl` when it is unset.                                                                                                                                                                           |
| `HubPath`                | `string`   | `/pipeline-hub`         | Hub path, starting with `/`.                                                                                                                                                                                                                                                                                               |
| `AccessToken`            | `string?`  | `null`                  | Token workers present to the master. When unset, the master generates one whenever it is reachable beyond the machine (a tunnel or a non-loopback `ListenUrl`) and shares it through master discovery; without discovery such a master fails at startup until every process configures a token. The token is never logged. |
| `ConnectionTimeout`      | `TimeSpan` | `00:02:00`              | How long a worker keeps trying to connect.                                                                                                                                                                                                                                                                                 |
| `MaxReconnectAttempts`   | `int`      | `5`                     | Automatic reconnect attempts with exponential backoff; `0` disables reconnecting. A worker that stays disconnected for longer than `WorkerTimeout` loses its leases.                                                                                                                                                       |
| `KeepAliveInterval`      | `TimeSpan` | `00:00:05`              | Ping interval on both sides.                                                                                                                                                                                                                                                                                               |
| `PeerTimeout`            | `TimeSpan` | `00:00:15`              | Silence after which a side considers its peer gone; at least twice `KeepAliveInterval`.                                                                                                                                                                                                                                    |
| `MaxMessageSizeBytes`    | `long`     | `1048576`               | Largest SignalR message, such as a module result.                                                                                                                                                                                                                                                                          |
| `Tunnel.Enabled`         | `bool`     | `false`                 | Start a cloudflared quick tunnel to the bound URL and advertise its public URL.                                                                                                                                                                                                                                            |
| `Tunnel.CloudflaredPath` | `string`   | `cloudflared`           | Path to the cloudflared binary.                                                                                                                                                                                                                                                                                            |
| `Tunnel.StartupTimeout`  | `TimeSpan` | `00:00:30`              | How long to wait for the tunnel URL.                                                                                                                                                                                                                                                                                       |

The hub rejects registrations for another `RunId`, a second live worker with the same `WorkerId`, and every call from a connection that has not registered; results and heartbeats must come from the registered worker. Detailed hub errors are disabled, and the tunnel URL is logged only at Debug.

## Redis Master Discovery[​](#redis-master-discovery "Direct link to Redis Master Discovery")

`AddRedisMasterDiscovery` stores the SignalR master's `MasterEndpoint`, its URL and access token, in Redis so workers can find it. Discovery opens its own lazily connected Redis connection; it neither uses nor replaces an `IConnectionMultiplexer` registered by the application.

| Property                | Type                            | Default          | Description                                                                  |
| ----------------------- | ------------------------------- | ---------------- | ---------------------------------------------------------------------------- |
| `ConnectionString`      | `string`                        | `localhost:6379` | Redis connection string; required unless `RestUrl` is set.                   |
| `ConfigureConnection`   | `Action<ConfigurationOptions>?` | `null`           | Adjusts the parsed connection configuration, for example credentials or TLS. |
| `RestUrl` / `RestToken` | `string?`                       | `null`           | Upstash REST endpoint and token; configure both or neither.                  |
| `KeyPrefix`             | `string`                        | `modpipe`        | Prefix of the endpoint key `{KeyPrefix}:{RunId}:master-endpoint`.            |
| `TimeToLive`            | `TimeSpan`                      | `01:00:00`       | How long the advertised endpoint is kept.                                    |
| `DiscoveryTimeout`      | `TimeSpan`                      | `00:02:00`       | How long workers wait for the endpoint.                                      |
| `PollInterval`          | `TimeSpan`                      | `00:00:00.500`   | How often workers check for the endpoint.                                    |

Because the stored endpoint includes the access token, only the run's processes should be able to read the discovery Redis database.

## Connection String Examples[​](#connection-string-examples "Direct link to Connection String Examples")

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

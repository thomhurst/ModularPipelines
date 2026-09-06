# Configuration

Distributed mode has two layers of configuration: the core `DistributedOptions` (shared across all coordinator implementations) and coordinator-specific options like `RedisDistributedOptions`.

## DistributedOptions[​](#distributedoptions "Direct link to DistributedOptions")

Passed to `AddDistributedMode()`. Controls the fundamental behavior of the master/worker system.

```
builder.AddDistributedMode(o =>

{

    o.InstanceIndex = 0;

    o.TotalInstances = 4;

    o.Role = DistributedRole.Master;

    o.Capabilities = [Capability.Docker, Capability.Gpu];

    o.RunId = Environment.GetEnvironmentVariable("MODULARPIPELINES_RUN_ID")!;

    o.CapabilityTimeout = TimeSpan.FromMinutes(5);

    o.MinimumWorkerCount = 0;

    o.ModuleResultTimeout = TimeSpan.FromMinutes(45);

    o.AutoDetectOsCapability = true;

});
```

| Property                 | Type                        | Default                                                 | Description                                                                                                                                                                                                   |
| ------------------------ | --------------------------- | ------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Role`                   | `DistributedRole`           | `Auto`                                                  | Explicit `Master` or `Worker` role. `Auto` derives the role from `InstanceIndex`.                                                                                                                             |
| `InstanceIndex`          | `int`                       | `0`                                                     | This instance's unique index. With `Role == Auto`, `0` selects master and values above `0` select worker.                                                                                                     |
| `TotalInstances`         | `int`                       | `1`                                                     | Total number of instances (master + workers).                                                                                                                                                                 |
| `Capabilities`           | `IReadOnlyList<Capability>` | `[]`                                                    | Capabilities this worker advertises. Built-in values are available from `Capability`; strings convert implicitly for custom values.                                                                           |
| `RunId`                  | `string`                    | `MODULARPIPELINES_RUN_ID` or generated for one instance | Identifier shared by every process in this pipeline run. Multi-instance runs fail fast when neither source is configured.                                                                                     |
| `RequireExplicitRunId`   | `bool`                      | `false`                                                 | Reject generated single-instance IDs. Shared Redis backends enable this automatically. S3 artifact-store registrations also require an explicit shared `RunId`, including for single-instance configurations. |
| `CapabilityTimeout`      | `TimeSpan`                  | `TimeSpan.FromMinutes(5)`                               | Registration grace period before an assignment with no capable worker fails with an explicit routing error.                                                                                                   |
| `MinimumWorkerCount`     | `int`                       | `0`                                                     | Number of external workers required before dispatch starts. Keep zero for immediate dispatch; set `TotalInstances - 1` for the former full-worker barrier.                                                    |
| `ModuleResultTimeout`    | `TimeSpan`                  | `TimeSpan.FromMinutes(45)`                              | Default maximum time to wait for a distributed module result. Use `TimeSpan.Zero` to wait indefinitely.                                                                                                       |
| `AutoDetectOsCapability` | `bool`                      | `true`                                                  | Automatically add the current OS as a capability (`"windows"`, `"linux"`, `"macos"`, or `"freebsd"`).                                                                                                         |

### Configuration from appsettings.json[​](#configuration-from-appsettingsjson "Direct link to Configuration from appsettings.json")

You can also bind from configuration:

```
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

```
builder.AddDistributedMode(builder.Configuration.GetSection("Distributed"));
```

Or call `builder.AddDistributedMode()` to bind the standard environment variables:

| Environment variable               | Option                                 |
| ---------------------------------- | -------------------------------------- |
| `MODULARPIPELINES_INSTANCE_INDEX`  | `InstanceIndex`                        |
| `MODULARPIPELINES_TOTAL_INSTANCES` | `TotalInstances`                       |
| `MODULARPIPELINES_RUN_ID`          | `RunId`                                |
| `MODULARPIPELINES_ROLE`            | `Role` (`Auto`, `Master`, or `Worker`) |

Configuration binding converts the string array to `Capability` values. Distributed wire payloads also remain plain JSON strings.

Satellite registrations use the same options pattern and accept configuration sections:

```
builder.AddRedisDistributedCoordinator(builder.Configuration.GetSection("Redis"));

builder.AddSignalRDistributedCoordinator(builder.Configuration.GetSection("SignalR"));

builder.AddRedisSignalRDiscovery(builder.Configuration.GetSection("RedisDiscovery"));

builder.AddS3DistributedArtifactStore(builder.Configuration.GetSection("S3"));
```

## RedisDistributedOptions[​](#redisdistributedoptions "Direct link to RedisDistributedOptions")

Passed to `AddRedisDistributedCoordinator()`. Controls how the Redis coordinator connects and manages keys.

```
builder.AddRedisDistributedCoordinator(o =>

{

    o.ConnectionString = "redis-host:6379,password=secret";

    o.KeyPrefix = "modpipe";

    o.KeyExpiration = TimeSpan.FromHours(1);

});
```

| Property           | Type       | Default                 | Description                                                                                                                   |
| ------------------ | ---------- | ----------------------- | ----------------------------------------------------------------------------------------------------------------------------- |
| `ConnectionString` | `string`   | `""`                    | StackExchange.Redis connection string. Supports all standard options (`password`, `ssl`, `abortConnect`, etc.). **Required.** |
| `KeyPrefix`        | `string`   | `"modpipe"`             | Prefix for all Redis keys. Change this if multiple different pipelines share the same Redis instance.                         |
| `KeyExpiration`    | `TimeSpan` | `TimeSpan.FromHours(1)` | TTL for all Redis keys. Keys are automatically cleaned up after this duration.                                                |

All distributed duration properties use `TimeSpan`. When binding them from `appsettings.json`, use the invariant `TimeSpan` string format:

```
{

  "Distributed": {

    "CapabilityTimeout": "00:05:00",

    "ModuleResultTimeout": "00:45:00"

  },

  "Redis": {

    "KeyExpiration": "01:00:00"

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

## Run Identifier Resolution[​](#run-identifier-resolution "Direct link to Run Identifier Resolution")

Distributed coordination requires an invocation-scoped identifier. It is resolved in this order:

| Priority | Source                            | Environment                     |
| -------- | --------------------------------- | ------------------------------- |
| 1        | `DistributedOptions.RunId`        | Explicit configuration          |
| 2        | `MODULARPIPELINES_RUN_ID` env var | Any CI or local orchestration   |
| 3        | Generated GUID                    | Single-process/default fallback |

Use an invocation-specific value rather than a stable commit identifier so rerunning the same commit receives a fresh Redis namespace. Local multi-process runs should export one unique `MODULARPIPELINES_RUN_ID` value before starting the master and workers. CI workflows must likewise generate or derive one invocation-specific value and export it as `MODULARPIPELINES_RUN_ID` for every master and worker.

## Redis Key Schema[​](#redis-key-schema "Direct link to Redis Key Schema")

All keys follow the pattern `{KeyPrefix}:{RunId}:{purpose}`. With the defaults, keys look like:

| Key                          | Redis Type | Purpose                                                  |
| ---------------------------- | ---------- | -------------------------------------------------------- |
| `modpipe:{run}:work:queue`   | List       | FIFO work queue for module assignments                   |
| `modpipe:{run}:results`      | Hash       | Completed module results (field = module type name)      |
| `modpipe:{run}:workers`      | Hash       | Registered worker information (field = worker index)     |
| `modpipe:{run}:heartbeats`   | Hash       | Worker heartbeat timestamps (field = worker index)       |
| `modpipe:{run}:cancellation` | String     | Cancellation signal (set when cancellation is broadcast) |

Pub/Sub channels (no TTL, ephemeral):

| Channel                                  | Purpose                                                      |
| ---------------------------------------- | ------------------------------------------------------------ |
| `modpipe:{run}:results:{ModuleTypeName}` | Notifies the master when a specific module's result is ready |
| `modpipe:{run}:cancellation:signal`      | Notifies all instances of a cancellation request             |

All storage keys have the configured TTL applied, so they are automatically cleaned up even if the pipeline crashes.

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

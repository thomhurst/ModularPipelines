---
title: Distributed Redis Discovery Package
---

# Distributed Redis Discovery Package

`ModularPipelines.Distributed.Discovery.Redis` advertises and discovers a distributed master's endpoint
through Redis. It implements the transport-neutral `IMasterDiscovery` contract, whose `MasterEndpoint`
carries the master's URL and access token; SignalR uses it so workers can locate and authenticate to the
current master.

## Installation

```shell
dotnet add package ModularPipelines.Distributed.Discovery.Redis
```

## Configuration

```csharp
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Discovery.Redis;
using ModularPipelines.Distributed.SignalR;

var builder = Pipeline.CreateBuilder(args);

builder.AddDistributedMode(options => options.TotalInstances = 2);
builder.AddSignalRDistributedCoordinator(options => options.Tunnel.Enabled = true);
builder.AddRedisMasterDiscovery(options =>
{
    options.ConnectionString = "localhost:6379";
});
```

Discovery has no default connection. Set `ConnectionString` explicitly, including `localhost:6379` for local Redis,
or supply endpoints through `ConfigureConnection`. Missing connection configuration fails during pipeline construction.
TCP options validation runs `ConfigureConnection` and requires at least one endpoint afterward.
The resulting configuration is cached per options instance; finish configuring options before building the pipeline.
Discovery opens its own Redis connection on first use. REST discovery does not invoke the TCP callback.
For REST-backed Redis services, configure both `RestUrl` and `RestToken`; no TCP connection string is needed:

```csharp
builder.AddRedisMasterDiscovery(options =>
{
    options.RestUrl = builder.Configuration["RedisRestUrl"];
    options.RestToken = builder.Configuration["RedisRestToken"];
});
```

For callback-only TCP configuration, supply endpoints explicitly:

```csharp
builder.AddRedisMasterDiscovery(options =>
{
    options.ConfigureConnection = connection =>
    {
        connection.EndPoints.Add("redis.internal", 6380);
        connection.Ssl = true;
        connection.Password = builder.Configuration["RedisPassword"];
    };
});
```

Because the stored endpoint includes the access token, only the run's processes should be able to read the
discovery database. See [Configuration](../distributed/configuration#redis-master-discovery) for all options.

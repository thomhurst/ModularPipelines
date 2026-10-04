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

Discovery opens its own Redis connection on first use; use `ConfigureConnection` to adjust credentials or TLS.
For REST-backed Redis services, configure both `RestUrl` and `RestToken`; they must be supplied together.
`RestUrl` is an absolute HTTP/HTTPS `Uri`, for example `options.RestUrl = new Uri("https://redis.example");`.
Because the stored endpoint includes the access token, only the run's processes should be able to read the
discovery database. See [Configuration](../distributed/configuration#redis-master-discovery) for all options.

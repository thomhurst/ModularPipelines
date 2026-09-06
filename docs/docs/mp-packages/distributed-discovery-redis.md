---
title: Distributed Redis Discovery Package
---

# Distributed Redis Discovery Package

`ModularPipelines.Distributed.Discovery.Redis` advertises and discovers a distributed master's endpoint through Redis. It implements the transport-neutral `IMasterDiscovery` contract; SignalR can consume that contract when workers need to locate the current master.

## Installation

```shell
dotnet add package ModularPipelines.Distributed.Discovery.Redis
```

## Configuration

Register Redis discovery with a coordinator that supports `IMasterDiscovery`. For SignalR:

```csharp
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Discovery.Redis;
using ModularPipelines.Distributed.SignalR;

var builder = Pipeline.CreateBuilder(args);

builder.AddDistributedMode(options => options.TotalInstances = 2);
builder.AddSignalRDistributedCoordinator();
builder.AddRedisMasterDiscovery(options =>
{
    options.ConnectionString = "localhost:6379";
});
```

For REST-backed Redis services, configure both `RestUrl` and `RestToken`; they must be supplied together.

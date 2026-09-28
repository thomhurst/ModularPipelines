---
title: Distributed SignalR Package
---

# Distributed SignalR Package

`ModularPipelines.Distributed.SignalR` provides SignalR-based coordination between distributed pipeline
workers. The master hosts a SignalR hub; workers connect to it and pull work, so they need no shared
database.

## Installation

```shell
dotnet add package ModularPipelines.Distributed.SignalR
```

## Configuration

```csharp
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.SignalR;

var builder = Pipeline.CreateBuilder(args);

builder.AddDistributedMode(options => options.TotalInstances = 2);
builder.AddSignalRDistributedCoordinator(options =>
{
    options.ListenUrl = "http://0.0.0.0:5099";
    options.AdvertisedUrl = new Uri("https://pipeline-master.example.com");
    options.AccessToken = Environment.GetEnvironmentVariable("PIPELINE_HUB_TOKEN");
});
```

Every process needs the same `RunId` (set it or export `MODULARPIPELINES_RUN_ID`). A master that is
reachable beyond its machine requires an access token: configure `AccessToken` on every process, or pair
the package with a discovery provider such as `ModularPipelines.Distributed.Discovery.Redis`, which shares a
generated token with the workers. See [Configuration](../distributed/configuration#signalr-coordinator) for
all options.

---
title: Distributed SignalR Package
---

# Distributed SignalR Package

`ModularPipelines.Distributed.SignalR` provides SignalR-based coordination between distributed pipeline workers.

## Installation

```shell
dotnet add package ModularPipelines.Distributed.SignalR
```

## Configuration

Register the coordinator after enabling distributed mode:

```csharp
using ModularPipelines.Distributed.SignalR.Extensions;
using ModularPipelines.Distributed.Extensions;

var builder = Pipeline.CreateBuilder(args);

builder.AddDistributedMode(options => options.TotalInstances = 2);
builder.AddSignalRDistributedCoordinator(options =>
{
    options.MasterUrl = "https://pipeline-master.example.com";
});
```

Pair this package with a discovery provider when workers cannot receive the master URL through static configuration.

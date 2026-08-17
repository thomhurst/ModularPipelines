# Distributed SignalR Package

`ModularPipelines.Distributed.SignalR` provides SignalR-based coordination between distributed pipeline workers.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Distributed.SignalR
```

## Configuration[​](#configuration "Direct link to Configuration")

Register the coordinator after enabling distributed mode:

```
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

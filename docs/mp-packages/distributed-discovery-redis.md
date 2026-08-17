# Distributed Redis Discovery Package

`ModularPipelines.Distributed.Discovery.Redis` lets distributed SignalR workers discover the current master through Redis.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Distributed.Discovery.Redis
```

## Configuration[​](#configuration "Direct link to Configuration")

Register discovery after the SignalR coordinator:

```
using ModularPipelines.Distributed.Discovery.Redis;

using ModularPipelines.Distributed.Extensions;

using ModularPipelines.Distributed.SignalR.Extensions;



var builder = Pipeline.CreateBuilder(args);



builder.AddDistributedMode(options => options.TotalInstances = 2);

builder.AddSignalRDistributedCoordinator();

builder.AddRedisSignalRDiscovery(options =>

{

    options.ConnectionString = "localhost:6379";

});
```

For REST-backed Redis services, configure both `RestUrl` and `RestToken`; they must be supplied together.

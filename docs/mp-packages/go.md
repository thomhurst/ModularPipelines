# Go Package

Strongly typed Go toolchain commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Go
```

Required command-line tool: `go`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Go.Extensions`, then use this service from a module:

* `context.Go()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Go.Extensions;



public class UseGoModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var go = context.Go();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Go integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.

# Flux Package

Strongly typed Flux CLI commands for GitOps workflows.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Flux
```

Required command-line tool: `flux`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Flux.Extensions`, then use this service from a module:

* `context.Flux()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Flux.Extensions;



public class UseFluxModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var flux = context.Flux();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Flux integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.

# Skopeo Package

Strongly typed Skopeo container-image commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Skopeo
```

Required command-line tool: `skopeo`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Skopeo.Extensions`, then use this service from a module:

* `context.Skopeo()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Skopeo.Extensions;



public class UseSkopeoModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var skopeo = context.Skopeo();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Skopeo integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.

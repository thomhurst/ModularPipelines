# Syft Package

Strongly typed Syft software-bill-of-materials commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Syft
```

Required command-line tool: `syft`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Syft.Extensions`, then use this service from a module:

* `context.Syft()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Syft.Extensions;



public class UseSyftModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var syft = context.Syft();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Syft integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.

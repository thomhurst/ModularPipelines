# Chocolatey Package

Strongly typed Chocolatey package-management commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Chocolatey
```

Required command-line tool: `choco`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Chocolatey.Extensions`, then use this service from a module:

* `context.Choco()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Chocolatey.Extensions;



public class UseChocoModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var choco = context.Choco();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Choco integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.

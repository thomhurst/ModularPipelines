# Python Package

Strongly typed pip package-management commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Python
```

Required command-line tool: `pip`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Python.Extensions`, then use this service from a module:

* `context.Pip()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Python.Extensions;



public class UsePipModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var pip = context.Pip();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Pip integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.

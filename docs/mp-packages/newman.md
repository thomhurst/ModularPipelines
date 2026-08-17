# Newman Package

Strongly typed Newman commands for Postman collections.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Newman
```

Required command-line tool: `newman`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Newman.Extensions`, then use this service from a module:

* `context.Newman()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Newman.Extensions;



public class UseNewmanModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var newman = context.Newman();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Newman integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.

# Azure Package

Azure SDK helpers and strongly typed Azure CLI commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Azure
```

Required command-line tool: `az`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Azure.Extensions`, then use these services from a module:

* `context.Azure()`
* `context.Az()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Azure.Extensions;



public class UseAzureModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var azure = context.Azure();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Azure integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.

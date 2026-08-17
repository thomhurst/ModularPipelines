# Azure Pipelines Package

Azure Pipelines environment and build integration helpers.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Azure.Pipelines
```

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Azure.Pipelines.Extensions`, then use this service from a module:

* `context.AzurePipeline()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Azure.Pipelines.Extensions;



public class UseAzurePipelineModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var azurePipeline = context.AzurePipeline();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("AzurePipeline integration is ready");

    }

}
```

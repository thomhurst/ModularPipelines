# Azure Pipelines Package

Azure Pipelines environment and build integration helpers.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Azure.Pipelines
```

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.AzurePipeline`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseAzurePipelineModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var azurePipeline = context.Tools.AzurePipeline;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("AzurePipeline integration is ready");

        return None.Value;

    }

}
```

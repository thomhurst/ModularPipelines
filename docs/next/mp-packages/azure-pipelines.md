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

## V4 migration[​](#v4-migration "Direct link to V4 migration")

`AzurePipelineExtensions` now uses the `ModularPipelines.Azure.Pipelines` namespace. Replace imports of `.Extensions` with that package root namespace. The public `RegisterAzurePipelineContext` method remains available for generated registration and is hidden from IntelliSense.

Read Azure environment values through `context.Tools.AzurePipeline.EnvironmentVariables` instead of `Variables`, matching the TeamCity and GitHub context member name. Use the shared build-system context instead of the removed `IsRunningOnAzurePipelines` property:

```
using ModularPipelines;



var isAzurePipelines = context.Environment.BuildSystem.Is(BuildSystem.AzurePipelines);
```

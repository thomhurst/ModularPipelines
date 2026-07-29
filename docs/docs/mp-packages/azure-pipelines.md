---
title: Azure Pipelines Package
---

# Azure Pipelines Package

Azure Pipelines environment and build integration helpers.

## Installation

```shell
dotnet add package ModularPipelines.Azure.Pipelines
```

## Context entry points

Import `ModularPipelines.Azure.Pipelines.Extensions`, then use this service from a module:

- `context.AzurePipeline()`

## Module example

```csharp
using ModularPipelines.Azure.Pipelines.Extensions;

public class UseAzurePipelineModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var azurePipeline = context.AzurePipeline();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("AzurePipeline integration is ready");
        return None.Value;
    }
}
```

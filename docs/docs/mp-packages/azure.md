---
title: Azure Package
---

# Azure Package

Azure SDK helpers and strongly typed Azure CLI commands.

## Installation

```shell
dotnet add package ModularPipelines.Azure
```

Required command-line tool: `az`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Azure.Extensions`, then use these services from a module:

- `context.Azure()`
- `context.Az()`

## Module example

```csharp
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

Generated CLI command pages in this section list every available command and its options record.

---
title: Flux Package
---

# Flux Package

Strongly typed Flux CLI commands for GitOps workflows.

## Installation

```shell
dotnet add package ModularPipelines.Flux
```

Required command-line tool: `flux`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Flux.Extensions`, then use this service from a module:

- `context.Flux()`

## Module example

```csharp
using ModularPipelines.Flux.Extensions;

public class UseFluxModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var flux = context.Flux();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Flux integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

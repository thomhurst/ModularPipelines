---
title: yq Package
---

# yq Package

Strongly typed yq YAML-processing commands.

## Installation

```shell
dotnet add package ModularPipelines.Yq
```

Required command-line tool: `yq`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Yq.Extensions`, then use this service from a module:

- `context.Yq()`

## Module example

```csharp
using ModularPipelines.Yq.Extensions;

public class UseYqModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var yq = context.Yq();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Yq integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

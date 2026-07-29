---
title: Skopeo Package
---

# Skopeo Package

Strongly typed Skopeo container-image commands.

## Installation

```shell
dotnet add package ModularPipelines.Skopeo
```

Required command-line tool: `skopeo`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Skopeo.Extensions`, then use this service from a module:

- `context.Skopeo()`

## Module example

```csharp
using ModularPipelines.Skopeo.Extensions;

public class UseSkopeoModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var skopeo = context.Skopeo();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Skopeo integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

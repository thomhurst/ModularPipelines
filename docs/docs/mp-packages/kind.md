---
title: kind Package
---

# kind Package

Strongly typed kind commands for local Kubernetes clusters.

## Installation

```shell
dotnet add package ModularPipelines.Kind
```

Required command-line tool: `kind`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Kind.Extensions`, then use this service from a module:

- `context.Kind()`

## Module example

```csharp
using ModularPipelines.Kind.Extensions;

public class UseKindModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var kind = context.Kind();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Kind integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

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

public class UseKindModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var kind = context.Kind();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Kind integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

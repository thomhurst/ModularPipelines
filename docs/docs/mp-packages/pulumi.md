---
title: Pulumi Package
---

# Pulumi Package

Strongly typed Pulumi infrastructure commands.

## Installation

```shell
dotnet add package ModularPipelines.Pulumi
```

Required command-line tool: `pulumi`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Pulumi.Extensions`, then use this service from a module:

- `context.Pulumi()`

## Module example

```csharp
using ModularPipelines.Pulumi.Extensions;

public class UsePulumiModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var pulumi = context.Pulumi();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Pulumi integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

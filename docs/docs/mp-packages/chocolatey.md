---
title: Chocolatey Package
---

# Chocolatey Package

Strongly typed Chocolatey package-management commands.

## Installation

```shell
dotnet add package ModularPipelines.Chocolatey
```

Required command-line tool: `choco`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Chocolatey.Extensions`, then use this service from a module:

- `context.Choco()`

## Module example

```csharp
using ModularPipelines.Chocolatey.Extensions;

public class UseChocoModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var choco = context.Choco();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Choco integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

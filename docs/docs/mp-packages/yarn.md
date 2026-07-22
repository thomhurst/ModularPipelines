---
title: Yarn Package
---

# Yarn Package

Strongly typed Yarn package-management commands.

## Installation

```shell
dotnet add package ModularPipelines.Yarn
```

Required command-line tool: `yarn`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Yarn.Extensions`, then use this service from a module:

- `context.Yarn()`

## Module example

```csharp
using ModularPipelines.Yarn.Extensions;

public class UseYarnModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var yarn = context.Yarn();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Yarn integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

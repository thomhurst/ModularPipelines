---
title: Homebrew Package
---

# Homebrew Package

Strongly typed Homebrew package-management commands.

## Installation

```shell
dotnet add package ModularPipelines.Homebrew
```

Required command-line tool: `brew`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Homebrew.Extensions`, then use this service from a module:

- `context.Brew()`

## Module example

```csharp
using ModularPipelines.Homebrew.Extensions;

public class UseBrewModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var brew = context.Brew();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Brew integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

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

public class UseBrewModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var brew = context.Brew();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Brew integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

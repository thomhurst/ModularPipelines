---
title: Flyway Package
---

# Flyway Package

Strongly typed Flyway database migration commands.

## Installation

```shell
dotnet add package ModularPipelines.Flyway
```

Required command-line tool: `flyway`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Flyway.Extensions`, then use this service from a module:

- `context.Flyway()`

## Module example

```csharp
using ModularPipelines.Flyway.Extensions;

public class UseFlywayModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var flyway = context.Flyway();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Flyway integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

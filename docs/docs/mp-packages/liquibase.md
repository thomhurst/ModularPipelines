---
title: Liquibase Package
---

# Liquibase Package

Strongly typed Liquibase database change-management commands.

## Installation

```shell
dotnet add package ModularPipelines.Liquibase
```

Required command-line tool: `liquibase`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Liquibase.Extensions`, then use this service from a module:

- `context.Liquibase()`

## Module example

```csharp
using ModularPipelines.Liquibase.Extensions;

public class UseLiquibaseModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var liquibase = context.Liquibase();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Liquibase integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

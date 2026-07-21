---
title: Syft Package
---

# Syft Package

Strongly typed Syft software-bill-of-materials commands.

## Installation

```shell
dotnet add package ModularPipelines.Syft
```

Required command-line tool: `syft`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Syft.Extensions`, then use this service from a module:

- `context.Syft()`

## Module example

```csharp
using ModularPipelines.Syft.Extensions;

public class UseSyftModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var syft = context.Syft();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Syft integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

---
title: Packer Package
---

# Packer Package

Strongly typed Packer machine-image build commands.

## Installation

```shell
dotnet add package ModularPipelines.Packer
```

Required command-line tool: `packer`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Packer.Extensions`, then use this service from a module:

- `context.Packer()`

## Module example

```csharp
using ModularPipelines.Packer.Extensions;

public class UsePackerModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var packer = context.Packer();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Packer integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

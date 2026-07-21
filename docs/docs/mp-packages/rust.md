---
title: Rust Package
---

# Rust Package

Strongly typed Cargo commands for Rust projects.

## Installation

```shell
dotnet add package ModularPipelines.Rust
```

Required command-line tool: `cargo`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Rust.Extensions`, then use this service from a module:

- `context.Cargo()`

## Module example

```csharp
using ModularPipelines.Rust.Extensions;

public class UseCargoModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var cargo = context.Cargo();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Cargo integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

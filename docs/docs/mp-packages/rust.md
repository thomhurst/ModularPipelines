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
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Rust.Extensions;
using ModularPipelines.Rust.Options;

public class UseCargoModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Cargo().CheckAsync(
            new CargoCheckOptions
            {
                Quiet = true,
            },
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

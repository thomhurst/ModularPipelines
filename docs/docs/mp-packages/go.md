---
title: Go Package
---

# Go Package

Strongly typed Go toolchain commands.

## Installation

```shell
dotnet add package ModularPipelines.Go
```

Required command-line tool: `go`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Go.Extensions`, then use this service from a module:

- `context.Go()`

## Module example

```csharp
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Go.Extensions;
using ModularPipelines.Go.Options;

public class UseGoModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Go().Vet(
            new GoVetOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

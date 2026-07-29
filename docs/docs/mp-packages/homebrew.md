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
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Homebrew.Extensions;
using ModularPipelines.Homebrew.Options;

public class UseBrewModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Brew().ListAsync(
            new BrewListOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

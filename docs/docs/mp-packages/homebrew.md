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

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Brew`

## Module example

```csharp
using ModularPipelines;
using ModularPipelines.Homebrew.Options;

public class UseBrewModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.Brew.ListAsync(
            new BrewListOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

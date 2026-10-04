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

## Shared command options

`Debug`, `Quiet`, and `Verbose` are inherited from `BrewOptions`. Homebrew selects
its command first, so these flags follow the command path:

```csharp
var options = new BrewListOptions { Verbose = true };
// brew list --verbose
```

Existing initializers keep the same property names. Reflection code that inspects
only properties declared directly on a command record should include inherited
properties. Cask settings such as `Appdir` remain on the applicable command
records; they are not general Homebrew settings.

Generation reads `Homebrew::CLI::Parser.global_options` from the installed CLI,
separately from its cask-option table. See the [official global options](https://docs.brew.sh/Manpage#global-options)
and [global cask options](https://docs.brew.sh/Manpage#global-cask-options).

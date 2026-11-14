---
title: Yarn Package
---

# Yarn Package

Strongly typed Yarn package-management commands.

## Installation

```shell
dotnet add package ModularPipelines.Yarn
```

Required command-line tool: `yarn`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Yarn`

## Module example

```csharp
using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.Yarn.Options;

public class UseYarnModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.Yarn.InstallAsync(
            new YarnInstallOptions
            {
                Immutable = true,
            },
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands. Every command
inherits `Cwd` from `YarnOptions`. See the [generated Yarn reference](cli/yarn.md) for the command list.

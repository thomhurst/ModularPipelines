---
title: Python Package
---

# Python Package

Strongly typed pip package-management commands.

## Installation

```shell
dotnet add package ModularPipelines.Python
```

Required command-line tool: `pip`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Pip`

## Module example

```csharp
using ModularPipelines;
using ModularPipelines.Python.Options;

public class UsePipModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.Pip.FreezeAsync(
            new PipFreezeOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

## General options

All command records inherit pip's General Options from `PipOptions`. These render
before the subcommand, including `Python`, which selects the interpreter:

```csharp
new PipInstallOptions
{
    Python = "python3",
    RequireVirtualenv = true,
    TrustedHost = ["packages.example", "mirror.example"],
    RequirementSpecifier = ["example-package"],
};
```

`TrustedHost`, `ExistsAction`, `UseFeature`, and `UseDeprecated` accept collections
and repeat their switch for each value. When migrating a single string initializer,
wrap it in a collection expression. `Proxy` URLs are masked in command logs.
Install options such as `Target` and Package Index Options such as `IndexUrl`
remain command-specific and render after `install`.

The generated API reflects pip 25.3 and includes `LockAsync`. See the
[pip CLI reference](./cli/pip.md) for the complete command and global-option list.

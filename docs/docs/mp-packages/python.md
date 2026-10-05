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
`Quiet` and `Verbose` are nullable integer counts: `Verbose = 3` renders three
`--verbose` flags before the command, while zero or `null` omits the flag.
Negative counts are rejected. When migrating Boolean initializers, replace `true`
with `1` and `false` with `0` or `null`. Pip documents three useful verbosity or
quietness levels; its parser accepts additive counts rather than Boolean values.
Install options such as `Target` and Package Index Options such as `IndexUrl`
remain command-specific and render after `install`.
Dependency groups are repeatable on install, download, wheel, and lock commands:
`Group = ["development", "testing"]` renders two `--group` switches and can serve
as the command's complete input source. Null, empty, and whitespace-only entries
are rejected before starting pip, including when mixed with valid group names.

The generated API reflects pip 25.3 and includes `LockAsync`. See the
[pip CLI reference](./cli/pip.md) for the complete command and global-option list.

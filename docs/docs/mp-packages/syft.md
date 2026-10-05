---
title: Syft Package
---

# Syft Package

Strongly typed Syft software-bill-of-materials commands.

## Installation

```shell
dotnet add package ModularPipelines.Syft
```

Required command-line tool: `syft`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Syft`

## Module example

```csharp

public class UseSyftModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var syft = context.Tools.Syft;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Syft integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Shared configuration and logging

All command options inherit these settings from `SyftOptions`:

| Property | CLI option | Value |
| --- | --- | --- |
| `Config` | `--config`, `-c` | Repeated configuration-file paths |
| `Profile` | `--profile` | Repeated configuration-profile names |
| `Quiet` | `--quiet`, `-q` | Suppress logging when `true` |
| `Verbose` | `--verbose`, `-v` | Integer verbosity count, such as `2` for debug |

They render before the subcommand, including nested commands:

```csharp
var options = new SyftConfigLocationsOptions
{
    Config = ["syft.yaml", "team.yaml"],
    Profile = ["ci"],
    Verbose = 2,
    All = true,
};
```

This renders `syft --config=syft.yaml --config=team.yaml --profile=ci --verbose=2 config locations --all`.
Null settings are omitted; `Quiet = false` does not emit a flag. Verbosity retains
its integer value, so callers can request more than one level.

Scan output, source selection, and cataloger settings stay on their commands.
`SyftConfigOptions.Load` applies only to `config`, while
`SyftConfigLocationsOptions.All` applies only to `config locations`. Login
passwords stay local and retain secret masking.

### Migration

The four property names and types are unchanged. They now live on `SyftOptions`
instead of being redeclared by each generated command. Object initializers keep
working; reflection code using `DeclaredOnly` must include inherited properties.
The root command's scan flags are not universally inherited. This audit uses
Syft 1.54.0 and preserves existing command/version API coverage.

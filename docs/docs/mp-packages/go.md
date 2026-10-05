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

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Go`

## Module example

```csharp
using ModularPipelines;
using ModularPipelines.Go.Options;

public class UseGoModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.Go.VetAsync(
            new GoVetOptions { WorkingDirectory = "./src" },
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Working directory and flag scope

Every command inherits `WorkingDirectory` from `GoOptions`. It renders as the
case-sensitive `-C` option before the command path, for example
`go -C "source folder" mod graph -x`. Go changes directory before selecting the
toolchain and interpreting relative file and package arguments. This is a Go CLI
setting; it does not change the pipeline process's working directory.

Go documents `-C` in `go help build` and requires it to be the first flag. Other
shared build flags, such as `-race`, remain on the commands that support them.
Command-local flags and operands follow the command path. In `GoTestOptions`,
`LowerC` still means compile without running (`-c`), and `Args` stays after package
operands so its values are passed to the test binary.

Migrate older command-specific `C` or `UpperC` directory properties to
`WorkingDirectory`. Remove equivalent manual `-C` arguments when using the typed
property. No compatibility aliases are generated. The `VersionAsync` command
remains available and also inherits the directory setting.

See the [Go command reference](https://pkg.go.dev/cmd/go) and
[generated Go CLI reference](cli/go.md) for supported commands and options.

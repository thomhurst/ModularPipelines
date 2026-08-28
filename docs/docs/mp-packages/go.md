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
            new GoVetOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

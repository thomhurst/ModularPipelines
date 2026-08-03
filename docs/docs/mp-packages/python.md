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
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
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

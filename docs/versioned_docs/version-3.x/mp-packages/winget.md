---
title: WinGet Package
---

# WinGet Package

Strongly typed Windows Package Manager commands.

## Installation

```shell
dotnet add package ModularPipelines.WinGet
```

Required command-line tool: `winget`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.WinGet.Extensions`, then use this service from a module:

- `context.Winget()`

## Module example

```csharp
using ModularPipelines.WinGet.Extensions;

public class UseWingetModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var winget = context.Winget();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Winget integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

---
title: .NET Package
---

# .NET Package

Strongly typed .NET CLI commands, builders, and TRX test-result parsing.

## Installation

```shell
dotnet add package ModularPipelines.DotNet
```

Required command-line tool: `dotnet`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.DotNet.Extensions`, then use these services from a module:

- `context.DotNet()`
- `context.Trx()`

## Module example

```csharp
using ModularPipelines.DotNet.Extensions;

public class UseDotNetModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var dotNet = context.DotNet();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("DotNet integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

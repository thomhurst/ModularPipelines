---
title: Node.js Package
---

# Node.js Package

Node.js, npm, npx, nvm, and strongly typed pnpm helpers.

## Installation

```shell
dotnet add package ModularPipelines.Node
```

Required command-line tools: `node`, `pnpm`. They must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Node.Extensions`, then use these services from a module:

- `context.Node()`
- `context.Pnpm()`

## Module example

```csharp
using ModularPipelines.Node.Extensions;

public class UseNodeModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var node = context.Node();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Node integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

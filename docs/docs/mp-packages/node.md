---
title: Node.js Package
---

# Node.js Package

Node.js, npm, npx, nvm, and strongly typed pnpm helpers.

## Installation

```shell
dotnet add package ModularPipelines.Node
```

Required command-line tools: `node`, `npm`, `npx`, `nvm`, and `pnpm`. Install the tools used by your pipeline and make them available on `PATH`.

## Context entry points

Import `ModularPipelines.Node.Extensions`, then use these services from a module:

- `context.Node()`
- `context.Node().Npm`
- `context.Node().Npx`
- `context.Node().Nvm`
- `context.Pnpm()`

## Module example

```csharp
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Node.Extensions;

public class UseNodeModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Node().Version(cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

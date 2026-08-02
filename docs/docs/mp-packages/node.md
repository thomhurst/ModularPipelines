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

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Node`
- `context.Tools.Node.Npm`
- `context.Tools.Node.Npx`
- `context.Tools.Node.Nvm`
- `context.Tools.Pnpm`

## Module example

```csharp
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

public class UseNodeModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.Node.VersionAsync(cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

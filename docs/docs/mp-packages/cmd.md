---
title: Command Prompt Package
---

# Command Prompt Package

Helpers for executing Windows Command Prompt commands.

## Installation

```shell
dotnet add package ModularPipelines.Cmd
```

Required command-line tool: `cmd`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Cmd.Extensions`, then use this service from a module:

- `context.Cmd()`

## Module example

```csharp
using ModularPipelines.Cmd.Extensions;

public class UseCmdModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var cmd = context.Cmd();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Cmd integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

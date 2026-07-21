---
title: Podman Package
---

# Podman Package

Strongly typed Podman container-management commands.

## Installation

```shell
dotnet add package ModularPipelines.Podman
```

Required command-line tool: `podman`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Podman.Extensions`, then use this service from a module:

- `context.Podman()`

## Module example

```csharp
using ModularPipelines.Podman.Extensions;

public class UsePodmanModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var podman = context.Podman();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Podman integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

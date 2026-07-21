---
title: Docker Package
---

# Docker Package

Strongly typed Docker container, image, network, volume, and Buildx commands.

## Installation

```shell
dotnet add package ModularPipelines.Docker
```

Required command-line tool: `docker`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Docker.Extensions`, then use this service from a module:

- `context.Docker()`

## Module example

```csharp
using ModularPipelines.Docker.Extensions;

public class UseDockerModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var docker = context.Docker();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Docker integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

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
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Docker.Extensions;
using ModularPipelines.Docker.Options;

public class UseDockerModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Docker().Info(
            new DockerInfoOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

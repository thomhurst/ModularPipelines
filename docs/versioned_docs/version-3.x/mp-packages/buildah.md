---
title: Buildah Package
---

# Buildah Package

Strongly typed Buildah commands for building OCI container images.

## Installation

```shell
dotnet add package ModularPipelines.Buildah
```

Required command-line tool: `buildah`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Buildah.Extensions`, then use this service from a module:

- `context.Buildah()`

## Module example

```csharp
using ModularPipelines.Buildah.Extensions;

public class UseBuildahModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var buildah = context.Buildah();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Buildah integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

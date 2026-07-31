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

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Buildah`

## Module example

```csharp

public class UseBuildahModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var buildah = context.Tools.Buildah;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Buildah integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

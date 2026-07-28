---
title: Helm Package
---

# Helm Package

Strongly typed Helm package-management commands for Kubernetes.

## Installation

```shell
dotnet add package ModularPipelines.Helm
```

Required command-line tool: `helm`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Helm.Extensions`, then use this service from a module:

- `context.Helm()`

## Module example

```csharp
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Helm.Extensions;
using ModularPipelines.Helm.Options;

public class UseHelmModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Helm().Env(
            new HelmEnvOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

---
title: Kubernetes Package
---

# Kubernetes Package

Strongly typed kubectl and Kustomize commands.

## Installation

```shell
dotnet add package ModularPipelines.Kubernetes
```

Required command-line tools: `kubectl`, `kustomize`. They must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Kubernetes.Extensions`, then use these services from a module:

- `context.Kubernetes()`
- `context.Kustomize()`

## Module example

```csharp
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Kubernetes.Extensions;
using ModularPipelines.Kubernetes.Options;

public class UseKubernetesModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Kubernetes().Config.ViewAsync(
            new KubernetesConfigViewOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

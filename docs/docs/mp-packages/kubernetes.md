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
using ModularPipelines.Kubernetes.Extensions;

public class UseKubernetesModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var kubernetes = context.Kubernetes();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Kubernetes integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

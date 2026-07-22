---
title: minikube Package
---

# minikube Package

Strongly typed minikube commands for local Kubernetes clusters.

## Installation

```shell
dotnet add package ModularPipelines.Minikube
```

Required command-line tool: `minikube`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Minikube.Extensions`, then use this service from a module:

- `context.Minikube()`

## Module example

```csharp
using ModularPipelines.Minikube.Extensions;

public class UseMinikubeModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var minikube = context.Minikube();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Minikube integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

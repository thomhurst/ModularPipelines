---
title: Trivy Package
---

# Trivy Package

Strongly typed Trivy vulnerability and misconfiguration scanning commands.

## Installation

```shell
dotnet add package ModularPipelines.Trivy
```

Required command-line tool: `trivy`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Trivy.Extensions`, then use this service from a module:

- `context.Trivy()`

## Module example

```csharp
using ModularPipelines.Trivy.Extensions;

public class UseTrivyModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var trivy = context.Trivy();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Trivy integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

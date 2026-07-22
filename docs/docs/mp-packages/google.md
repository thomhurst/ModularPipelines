---
title: Google Cloud Package
---

# Google Cloud Package

Strongly typed Google Cloud CLI commands.

## Installation

```shell
dotnet add package ModularPipelines.Google
```

Required command-line tool: `gcloud`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Google.Extensions`, then use this service from a module:

- `context.Gcloud()`

## Module example

```csharp
using ModularPipelines.Google.Extensions;

public class UseGcloudModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var gcloud = context.Gcloud();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Gcloud integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

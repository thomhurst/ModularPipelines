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
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Google.Extensions;
using ModularPipelines.Google.Options;

public class UseGcloudModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Gcloud().Info(
            new GcloudInfoOptions
            {
                Anonymize = true,
            },
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

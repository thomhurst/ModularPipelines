---
title: Amazon Web Services Package
---

# Amazon Web Services Package

AWS CLI commands plus Amazon provisioning and S3 helpers.

## Installation

```shell
dotnet add package ModularPipelines.AmazonWebServices
```

Required command-line tool: `aws`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.AmazonWebServices.Extensions`, then use this service from a module:

- `context.Aws()`

## Module example

```csharp
using ModularPipelines.AmazonWebServices.Extensions;

public class UseAwsModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var aws = context.Aws();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Aws integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

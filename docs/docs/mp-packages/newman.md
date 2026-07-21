---
title: Newman Package
---

# Newman Package

Strongly typed Newman commands for Postman collections.

## Installation

```shell
dotnet add package ModularPipelines.Newman
```

Required command-line tool: `newman`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Newman.Extensions`, then use this service from a module:

- `context.Newman()`

## Module example

```csharp
using ModularPipelines.Newman.Extensions;

public class UseNewmanModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var newman = context.Newman();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Newman integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.

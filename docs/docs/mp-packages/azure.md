---
title: Azure Package
---

# Azure Package

Azure SDK helpers and strongly typed Azure CLI commands.

## Installation

```shell
dotnet add package ModularPipelines.Azure
```

Required command-line tool: `az`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Azure.Extensions`, then use these services from a module:

- `context.Azure()`
- `context.Az()`

## Module example

```csharp
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Azure.Options;

public class UseAzureModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Az().Account.ListAsync(
            new AzAccountListOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

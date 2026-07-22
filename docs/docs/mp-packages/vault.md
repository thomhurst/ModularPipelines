---
title: Vault Package
---

# Vault Package

Strongly typed HashiCorp Vault commands.

## Installation

```shell
dotnet add package ModularPipelines.Vault
```

Required command-line tool: `vault`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Vault.Extensions`, then use this service from a module:

- `context.Vault()`

## Module example

```csharp
using ModularPipelines.Vault.Extensions;

public class UseVaultModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var vault = context.Vault();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Vault integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

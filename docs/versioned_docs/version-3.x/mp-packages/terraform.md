---
title: Terraform Package
---

# Terraform Package

Strongly typed Terraform infrastructure commands.

## Installation

```shell
dotnet add package ModularPipelines.Terraform
```

Required command-line tool: `terraform`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Terraform.Extensions`, then use this service from a module:

- `context.Terraform()`

## Module example

```csharp
using ModularPipelines.Terraform.Extensions;

public class UseTerraformModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var terraform = context.Terraform();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Terraform integration is ready");
    }
}
```

The package exposes generated options records for its supported CLI commands.

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

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Terraform`

## Module example

```csharp
using ModularPipelines;
using ModularPipelines.Terraform.Options;

public class UseTerraformModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.Terraform.ValidateAsync(
            new TerraformValidateOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

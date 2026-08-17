# Terraform Package

Strongly typed Terraform infrastructure commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Terraform
```

Required command-line tool: `terraform`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Terraform.Extensions`, then use this service from a module:

* `context.Terraform()`

## Module example[​](#module-example "Direct link to Module example")

```
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

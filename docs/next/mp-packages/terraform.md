# Terraform Package

Strongly typed Terraform infrastructure commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Terraform
```

Required command-line tool: `terraform`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Terraform`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

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

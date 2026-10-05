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

## Working directory

Set the inherited `Chdir` property on any command options record to select Terraform's
working directory. The package renders `-chdir=DIR` before the entire subcommand path,
including nested commands such as `state list`.

```csharp
var options = new TerraformValidateOptions
{
    Chdir = "infrastructure/production",
    Json = true,
};
```

This renders `terraform -chdir=infrastructure/production validate -json`. Leave `Chdir`
unset to use the process working directory. Paths containing spaces remain one argument;
do not add shell quotes to the property value.

Terraform's root `-help` and `-version` switches select informational actions. They are
not inherited execution settings. Command-specific switches such as `-json` and
`-no-color` stay on the commands that support them. See the
[Terraform CLI documentation](https://developer.hashicorp.com/terraform/cli/commands#switching-working-directory-with-chdir)
for details of `-chdir`.

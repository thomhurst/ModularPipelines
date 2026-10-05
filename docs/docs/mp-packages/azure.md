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

Commands under `artifacts`, `boards`, `devops`, `pipelines`, and `repos` also require the Azure DevOps extension:

```shell
az extension add --name azure-devops
```

For automated DevOps authentication, obtain a PAT from your secret provider and pass it as
`AZURE_DEVOPS_EXT_PAT` in `CommandExecutionOptions.EnvironmentVariables`. This authenticates
DevOps commands without calling `az devops login`. Never put the PAT in command arguments.
See [Microsoft's PAT authentication guidance](https://learn.microsoft.com/azure/devops/cli/log-in-via-pat).

`az devops login` itself requires a terminal prompt or a PAT supplied through stdin.
The public command execution API exposes neither terminal interaction nor stdin, so this
command is excluded from generated coverage. To populate the CLI credential store, run it
separately before the pipeline. Environment-variable authentication does not populate that store.

## Azure CLI argument types

Generated option types follow the installed Azure CLI parser. VM SKU values such as
`Standard_DS2_v2`, and count-or-percentage values such as `5%`, remain strings. Integer
parser arguments remain numeric.

Arguments accepting zero or more values use `IEnumerable<CliOptionValue>`. For example,
container storage can use `[CliOptionValue.Bare]` for the bare switch or `["pool-name"]`
for a value. DevOps parameters, variables, and work-item fields accept grouped values such
as `["configuration=Release", "platform=x64"]`. Null omits the switch; an empty collection
also emits nothing. Use `CliOptionValue.Bare` explicitly when a bare switch is intended.

Repeatable multi-value arguments use `IEnumerable<CliValueGroup>` to retain each occurrence.
For example, set `AssignPlatformWi` to
`[new CliValueGroup(["operator-one", "identity-one"]), new CliValueGroup(["operator-two", "identity-two"])]`.

When migrating, follow the generated parser contract rather than the option name:
`AzSynapseSparkJobSubmitOptions.Executors` takes a string such as `"1"`;
`AzManagedCassandraClusterInvokeCommandOptions.ClusterArguments` takes groups;
`AzMonitorAccountIssueUpdateOptions.ForceString` takes `"true"`, `"false"`, or
`CliOptionValue.Bare`. An unset optional value emits nothing. A grouped assignment
keeps all its operands together, and each additional group repeats the switch.
This emits `--assign-platform-wi operator-one identity-one` followed by
`--assign-platform-wi operator-two identity-two`. Each group holds separate
argument values; do not join them into a shell string. Empty groups are accepted only for
options whose parser allows a bare occurrence.

Generation uses an isolated Azure configuration and extension directory containing the
explicitly installed Azure DevOps extension. The coverage manifest records the CLI and
extension versions without machine-specific paths or Python installation details.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Azure`
- `context.Tools.Az`

## Module example

```csharp
using ModularPipelines;
using ModularPipelines.Azure.Options;

public class UseAzureModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.Az.Account.ListAsync(
            new AzAccountListOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

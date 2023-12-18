using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "sql", "pool", "classification", "create")]
public record AzSynapseSqlPoolClassificationCreateOptions(
[property: CommandSwitch("--column")] string Column,
[property: CommandSwitch("--information-type")] string InformationType,
[property: CommandSwitch("--label")] string Label,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--schema")] string Schema,
[property: CommandSwitch("--table")] string Table,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
}
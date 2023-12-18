using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "operation", "group", "show")]
public record AzDeploymentOperationGroupShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--operation-ids")] string OperationIds,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}


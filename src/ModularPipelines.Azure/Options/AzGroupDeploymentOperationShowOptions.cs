using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("group", "deployment", "operation", "show")]
public record AzGroupDeploymentOperationShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--operation-ids")] string OperationIds,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "operation", "tenant", "show")]
public record AzDeploymentOperationTenantShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--operation-ids")] string OperationIds
) : AzOptions
{
}
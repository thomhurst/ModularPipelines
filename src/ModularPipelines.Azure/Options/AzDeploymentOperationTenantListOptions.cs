using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "operation", "tenant", "list")]
public record AzDeploymentOperationTenantListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}
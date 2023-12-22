using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "operation", "tenant", "list")]
public record AzDeploymentOperationTenantListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;
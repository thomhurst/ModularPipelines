using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "tenant", "show")]
public record AzDeploymentTenantShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;
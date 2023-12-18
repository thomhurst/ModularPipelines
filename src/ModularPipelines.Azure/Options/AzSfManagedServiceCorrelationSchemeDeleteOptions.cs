using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-service", "correlation-scheme", "delete")]
public record AzSfManagedServiceCorrelationSchemeDeleteOptions(
[property: CommandSwitch("--application")] string Application,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--correlated-name")] string CorrelatedName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}
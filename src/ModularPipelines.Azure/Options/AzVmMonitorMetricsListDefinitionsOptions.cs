using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "monitor", "metrics", "list-definitions")]
public record AzVmMonitorMetricsListDefinitionsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }
}
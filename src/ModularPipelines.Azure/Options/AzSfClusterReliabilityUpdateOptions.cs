using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "cluster", "reliability", "update")]
public record AzSfClusterReliabilityUpdateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--reliability-level")] string ReliabilityLevel,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--auto-add-node")]
    public bool? AutoAddNode { get; set; }
}
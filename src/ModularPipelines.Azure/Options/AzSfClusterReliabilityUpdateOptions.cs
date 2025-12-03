using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "cluster", "reliability", "update")]
public record AzSfClusterReliabilityUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--reliability-level")] string ReliabilityLevel,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--auto-add-node")]
    public bool? AutoAddNode { get; set; }
}
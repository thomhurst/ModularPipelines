using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "clusterupgrade", "describe")]
public record GcloudContainerFleetClusterupgradeDescribeOptions : GcloudOptions
{
    [CliFlag("--show-linked-cluster-upgrade")]
    public bool? ShowLinkedClusterUpgrade { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "clusterupgrade", "describe")]
public record GcloudContainerHubClusterupgradeDescribeOptions : GcloudOptions
{
    [CliFlag("--show-linked-cluster-upgrade")]
    public bool? ShowLinkedClusterUpgrade { get; set; }
}
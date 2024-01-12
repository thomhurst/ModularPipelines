using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "clusterupgrade", "describe")]
public record GcloudContainerHubClusterupgradeDescribeOptions : GcloudOptions
{
    [BooleanCommandSwitch("--show-linked-cluster-upgrade")]
    public bool? ShowLinkedClusterUpgrade { get; set; }
}
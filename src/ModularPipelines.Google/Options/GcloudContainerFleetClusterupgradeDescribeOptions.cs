using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "clusterupgrade", "describe")]
public record GcloudContainerFleetClusterupgradeDescribeOptions : GcloudOptions
{
    [BooleanCommandSwitch("--show-linked-cluster-upgrade")]
    public bool? ShowLinkedClusterUpgrade { get; set; }
}
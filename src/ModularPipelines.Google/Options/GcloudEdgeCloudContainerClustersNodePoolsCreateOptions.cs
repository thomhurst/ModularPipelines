using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "clusters", "node-pools", "create")]
public record GcloudEdgeCloudContainerClustersNodePoolsCreateOptions(
[property: PositionalArgument] string NodePool,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--node-count")] string NodeCount,
[property: CommandSwitch("--node-location")] string NodeLocation
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--local-disk-kms-key")]
    public string? LocalDiskKmsKey { get; set; }

    [CommandSwitch("--lro-timeout")]
    public string? LroTimeout { get; set; }

    [CommandSwitch("--machine-filter")]
    public string? MachineFilter { get; set; }
}
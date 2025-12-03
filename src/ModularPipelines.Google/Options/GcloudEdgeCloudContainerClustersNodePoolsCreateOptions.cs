using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "clusters", "node-pools", "create")]
public record GcloudEdgeCloudContainerClustersNodePoolsCreateOptions(
[property: CliArgument] string NodePool,
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliOption("--node-count")] string NodeCount,
[property: CliOption("--node-location")] string NodeLocation
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--local-disk-kms-key")]
    public string? LocalDiskKmsKey { get; set; }

    [CliOption("--lro-timeout")]
    public string? LroTimeout { get; set; }

    [CliOption("--machine-filter")]
    public string? MachineFilter { get; set; }
}
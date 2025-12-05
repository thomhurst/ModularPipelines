using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "create")]
public record GcloudBigtableInstancesCreateOptions(
[property: CliArgument] string Instance,
[property: CliOption("--display-name")] string DisplayName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--cluster-config")]
    public string[]? ClusterConfig { get; set; }

    [CliOption("--cluster-num-nodes")]
    public string? ClusterNumNodes { get; set; }

    [CliOption("--cluster-storage-type")]
    public string? ClusterStorageType { get; set; }

    [CliOption("--cluster-zone")]
    public string? ClusterZone { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }
}
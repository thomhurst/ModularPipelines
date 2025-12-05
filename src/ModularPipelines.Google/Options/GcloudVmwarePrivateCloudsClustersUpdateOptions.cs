using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "clusters", "update")]
public record GcloudVmwarePrivateCloudsClustersUpdateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--node-type-config")]
    public string[]? NodeTypeConfig { get; set; }

    [CliOption("--remove-nodes-config")]
    public string? RemoveNodesConfig { get; set; }

    [CliOption("--update-nodes-config")]
    public string[]? UpdateNodesConfig { get; set; }
}
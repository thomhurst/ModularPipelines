using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "clusters", "update")]
public record GcloudVmwarePrivateCloudsClustersUpdateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--node-type-config")]
    public string[]? NodeTypeConfig { get; set; }

    [CommandSwitch("--remove-nodes-config")]
    public string? RemoveNodesConfig { get; set; }

    [CommandSwitch("--update-nodes-config")]
    public string[]? UpdateNodesConfig { get; set; }
}
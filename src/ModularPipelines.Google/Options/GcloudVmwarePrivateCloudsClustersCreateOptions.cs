using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "clusters", "create")]
public record GcloudVmwarePrivateCloudsClustersCreateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud,
[property: CommandSwitch("--node-type-config")] string[] NodeTypeConfig
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}
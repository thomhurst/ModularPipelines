using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "clusters", "create")]
public record GcloudVmwarePrivateCloudsClustersCreateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud,
[property: CliOption("--node-type-config")] string[] NodeTypeConfig
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}
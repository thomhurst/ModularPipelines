using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "clusters", "nodes", "list")]
public record GcloudVmwarePrivateCloudsClustersNodesListOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--location")] string Location,
[property: CliOption("--private-cloud")] string PrivateCloud
) : GcloudOptions;
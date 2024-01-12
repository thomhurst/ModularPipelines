using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "clusters", "nodes", "list")]
public record GcloudVmwarePrivateCloudsClustersNodesListOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--private-cloud")] string PrivateCloud
) : GcloudOptions;
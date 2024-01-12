using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "clusters", "nodes", "describe")]
public record GcloudVmwarePrivateCloudsClustersNodesDescribeOptions(
[property: PositionalArgument] string Node,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud
) : GcloudOptions;
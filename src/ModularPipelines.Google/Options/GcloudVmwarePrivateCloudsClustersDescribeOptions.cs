using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "clusters", "describe")]
public record GcloudVmwarePrivateCloudsClustersDescribeOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud
) : GcloudOptions;
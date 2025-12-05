using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "clusters", "describe")]
public record GcloudVmwarePrivateCloudsClustersDescribeOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "clusters", "describe")]
public record GcloudEdgeCloudContainerClustersDescribeOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions;
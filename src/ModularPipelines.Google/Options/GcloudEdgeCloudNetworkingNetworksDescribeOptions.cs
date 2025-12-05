using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "networks", "describe")]
public record GcloudEdgeCloudNetworkingNetworksDescribeOptions(
[property: CliArgument] string Network,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions;
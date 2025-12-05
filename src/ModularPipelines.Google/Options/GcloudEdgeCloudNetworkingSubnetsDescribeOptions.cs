using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "subnets", "describe")]
public record GcloudEdgeCloudNetworkingSubnetsDescribeOptions(
[property: CliArgument] string Subnet,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions;
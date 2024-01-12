using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "subnets", "describe")]
public record GcloudEdgeCloudNetworkingSubnetsDescribeOptions(
[property: PositionalArgument] string Subnet,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone
) : GcloudOptions;
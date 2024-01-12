using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "routers", "describe")]
public record GcloudEdgeCloudNetworkingRoutersDescribeOptions(
[property: PositionalArgument] string Router,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "routers", "describe")]
public record GcloudEdgeCloudNetworkingRoutersDescribeOptions(
[property: CliArgument] string Router,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions;
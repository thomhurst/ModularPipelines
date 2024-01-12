using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "routers", "get-status")]
public record GcloudEdgeCloudNetworkingRoutersGetStatusOptions(
[property: PositionalArgument] string Router,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone
) : GcloudOptions;
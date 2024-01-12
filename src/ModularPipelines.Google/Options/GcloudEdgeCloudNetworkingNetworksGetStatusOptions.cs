using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "networks", "get-status")]
public record GcloudEdgeCloudNetworkingNetworksGetStatusOptions(
[property: PositionalArgument] string Network,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone
) : GcloudOptions;
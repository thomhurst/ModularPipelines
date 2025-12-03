using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "networks", "get-status")]
public record GcloudEdgeCloudNetworkingNetworksGetStatusOptions(
[property: CliArgument] string Network,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions;
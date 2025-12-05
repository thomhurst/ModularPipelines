using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "networks", "list")]
public record GcloudEdgeCloudNetworkingNetworksListOptions(
[property: CliOption("--zone")] string Zone,
[property: CliOption("--location")] string Location
) : GcloudOptions;
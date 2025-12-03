using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "routers", "list")]
public record GcloudEdgeCloudNetworkingRoutersListOptions(
[property: CliOption("--zone")] string Zone,
[property: CliOption("--location")] string Location
) : GcloudOptions;
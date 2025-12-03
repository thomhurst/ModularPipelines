using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "zones", "init")]
public record GcloudEdgeCloudNetworkingZonesInitOptions(
[property: CliArgument] string Zone,
[property: CliArgument] string Location
) : GcloudOptions;
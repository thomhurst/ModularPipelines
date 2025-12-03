using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "interconnects", "list")]
public record GcloudEdgeCloudNetworkingInterconnectsListOptions(
[property: CliOption("--zone")] string Zone,
[property: CliOption("--location")] string Location
) : GcloudOptions;
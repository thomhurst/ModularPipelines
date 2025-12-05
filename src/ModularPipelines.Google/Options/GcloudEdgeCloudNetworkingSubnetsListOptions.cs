using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "subnets", "list")]
public record GcloudEdgeCloudNetworkingSubnetsListOptions(
[property: CliOption("--zone")] string Zone,
[property: CliOption("--location")] string Location
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "interconnects", "describe")]
public record GcloudEdgeCloudNetworkingInterconnectsDescribeOptions(
[property: CliArgument] string Interconnect,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions;
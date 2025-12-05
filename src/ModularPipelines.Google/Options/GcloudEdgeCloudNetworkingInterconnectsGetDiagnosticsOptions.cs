using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "interconnects", "get-diagnostics")]
public record GcloudEdgeCloudNetworkingInterconnectsGetDiagnosticsOptions(
[property: CliArgument] string Interconnect,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions;
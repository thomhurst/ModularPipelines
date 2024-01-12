using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "interconnects", "get-diagnostics")]
public record GcloudEdgeCloudNetworkingInterconnectsGetDiagnosticsOptions(
[property: PositionalArgument] string Interconnect,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone
) : GcloudOptions;
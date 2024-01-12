using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "zones", "init")]
public record GcloudEdgeCloudNetworkingZonesInitOptions(
[property: PositionalArgument] string Zone,
[property: PositionalArgument] string Location
) : GcloudOptions;
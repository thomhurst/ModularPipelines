using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "networks", "list")]
public record GcloudEdgeCloudNetworkingNetworksListOptions(
[property: CommandSwitch("--zone")] string Zone,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;
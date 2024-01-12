using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "routers", "list")]
public record GcloudEdgeCloudNetworkingRoutersListOptions(
[property: CommandSwitch("--zone")] string Zone,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;
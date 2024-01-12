using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "interconnects", "list")]
public record GcloudEdgeCloudNetworkingInterconnectsListOptions(
[property: CommandSwitch("--zone")] string Zone,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;
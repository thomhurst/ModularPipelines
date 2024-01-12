using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "subnets", "list")]
public record GcloudEdgeCloudNetworkingSubnetsListOptions(
[property: CommandSwitch("--zone")] string Zone,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;
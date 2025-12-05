using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "probe", "list")]
public record AzNetworkFrontDoorProbeListOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;
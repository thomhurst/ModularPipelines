using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "frontend-endpoint", "show")]
public record AzNetworkFrontDoorFrontendEndpointShowOptions(
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
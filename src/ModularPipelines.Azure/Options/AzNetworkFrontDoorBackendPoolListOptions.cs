using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "backend-pool", "list")]
public record AzNetworkFrontDoorBackendPoolListOptions(
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
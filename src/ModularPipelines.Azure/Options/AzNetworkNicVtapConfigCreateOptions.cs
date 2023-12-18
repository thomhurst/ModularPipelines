using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nic", "vtap-config", "create")]
public record AzNetworkNicVtapConfigCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--nic-name")] string NicName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vnet-tap")] string VnetTap
) : AzOptions;
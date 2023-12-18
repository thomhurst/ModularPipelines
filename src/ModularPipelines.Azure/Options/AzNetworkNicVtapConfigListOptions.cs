using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nic", "vtap-config", "list")]
public record AzNetworkNicVtapConfigListOptions(
[property: CommandSwitch("--nic-name")] string NicName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
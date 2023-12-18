using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nic", "ip-config", "list")]
public record AzNetworkNicIpConfigListOptions(
[property: CommandSwitch("--nic-name")] string NicName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;
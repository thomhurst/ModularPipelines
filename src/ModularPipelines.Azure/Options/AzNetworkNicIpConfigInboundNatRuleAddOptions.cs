using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nic", "ip-config", "inbound-nat-rule", "add")]
public record AzNetworkNicIpConfigInboundNatRuleAddOptions(
[property: CommandSwitch("--inbound-nat-rule")] string InboundNatRule,
[property: CommandSwitch("--ip-config-name")] string IpConfigName,
[property: CommandSwitch("--nic-name")] string NicName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--lb-name")]
    public string? LbName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}
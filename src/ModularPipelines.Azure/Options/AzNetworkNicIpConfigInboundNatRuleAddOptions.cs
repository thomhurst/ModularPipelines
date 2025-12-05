using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "nic", "ip-config", "inbound-nat-rule", "add")]
public record AzNetworkNicIpConfigInboundNatRuleAddOptions(
[property: CliOption("--inbound-nat-rule")] string InboundNatRule,
[property: CliOption("--ip-config-name")] string IpConfigName,
[property: CliOption("--nic-name")] string NicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--lb-name")]
    public string? LbName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}
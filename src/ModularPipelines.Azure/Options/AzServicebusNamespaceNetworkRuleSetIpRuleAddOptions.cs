using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "namespace", "network-rule-set", "ip-rule", "add")]
public record AzServicebusNamespaceNetworkRuleSetIpRuleAddOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ip-rule")]
    public string? IpRule { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "namespace", "network-rule-set", "virtual-network-rule", "remove")]
public record AzServicebusNamespaceNetworkRuleSetVirtualNetworkRuleRemoveOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }
}
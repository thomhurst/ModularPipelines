using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace", "network-rule-set", "virtual-network-rule", "remove")]
public record AzEventhubsNamespaceNetworkRuleSetVirtualNetworkRuleRemoveOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }
}
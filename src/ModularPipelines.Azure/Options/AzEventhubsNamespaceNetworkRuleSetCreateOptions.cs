using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace", "network-rule-set", "create")]
public record AzEventhubsNamespaceNetworkRuleSetCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--default-action")]
    public string? DefaultAction { get; set; }

    [BooleanCommandSwitch("--enable-trusted-service-access")]
    public bool? EnableTrustedServiceAccess { get; set; }

    [CommandSwitch("--ip-rules")]
    public string? IpRules { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--virtual-network-rules")]
    public string? VirtualNetworkRules { get; set; }
}
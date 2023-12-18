using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "namespace", "network-rule-set", "update")]
public record AzServicebusNamespaceNetworkRuleSetUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--default-action")]
    public string? DefaultAction { get; set; }

    [BooleanCommandSwitch("--enable-trusted-service-access")]
    public bool? EnableTrustedServiceAccess { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ip-rules")]
    public string? IpRules { get; set; }

    [CommandSwitch("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--virtual-network-rules")]
    public string? VirtualNetworkRules { get; set; }
}
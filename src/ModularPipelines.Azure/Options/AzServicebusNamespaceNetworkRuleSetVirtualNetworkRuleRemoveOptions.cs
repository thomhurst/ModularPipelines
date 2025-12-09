using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "namespace", "network-rule-set", "virtual-network-rule", "remove")]
public record AzServicebusNamespaceNetworkRuleSetVirtualNetworkRuleRemoveOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--subnet")]
    public string? Subnet { get; set; }
}
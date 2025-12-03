using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "namespace", "network-rule-set", "virtual-network-rule", "add")]
public record AzServicebusNamespaceNetworkRuleSetVirtualNetworkRuleAddOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--subnet")]
    public string? Subnet { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "namespace", "network-rule-set", "virtual-network-rule", "add")]
public record AzEventhubsNamespaceNetworkRuleSetVirtualNetworkRuleAddOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--subnet")]
    public string? Subnet { get; set; }
}
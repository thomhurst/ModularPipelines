using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "namespace", "network-rule-set", "ip-rule", "remove")]
public record AzServicebusNamespaceNetworkRuleSetIpRuleRemoveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--ip-rule")]
    public string? IpRule { get; set; }
}
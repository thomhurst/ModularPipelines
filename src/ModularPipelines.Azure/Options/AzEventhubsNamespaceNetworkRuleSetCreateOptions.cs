using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "namespace", "network-rule-set", "create")]
public record AzEventhubsNamespaceNetworkRuleSetCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliFlag("--enable-trusted-service-access")]
    public bool? EnableTrustedServiceAccess { get; set; }

    [CliOption("--ip-rules")]
    public string? IpRules { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--virtual-network-rules")]
    public string? VirtualNetworkRules { get; set; }
}
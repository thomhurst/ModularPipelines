using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventhubs", "namespace", "network-rule-set", "update")]
public record AzEventhubsNamespaceNetworkRuleSetUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliFlag("--enable-trusted-service-access")]
    public bool? EnableTrustedServiceAccess { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ip-rules")]
    public string? IpRules { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--virtual-network-rules")]
    public string? VirtualNetworkRules { get; set; }
}
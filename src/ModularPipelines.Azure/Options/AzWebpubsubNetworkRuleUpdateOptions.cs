using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webpubsub", "network-rule", "update")]
public record AzWebpubsubNetworkRuleUpdateOptions : AzOptions
{
    [CliFlag("--allow")]
    public bool? Allow { get; set; }

    [CliOption("--connection-name")]
    public string? ConnectionName { get; set; }

    [CliOption("--deny")]
    public string? Deny { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--public-network")]
    public bool? PublicNetwork { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
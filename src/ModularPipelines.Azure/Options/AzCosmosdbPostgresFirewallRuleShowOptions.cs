using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "postgres", "firewall-rule", "show")]
public record AzCosmosdbPostgresFirewallRuleShowOptions : AzOptions
{
    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--firewall-rule-name")]
    public string? FirewallRuleName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
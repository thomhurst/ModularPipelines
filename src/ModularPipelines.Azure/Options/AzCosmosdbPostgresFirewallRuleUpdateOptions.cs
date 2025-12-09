using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "postgres", "firewall-rule", "update")]
public record AzCosmosdbPostgresFirewallRuleUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--end-ip-address")]
    public string? EndIpAddress { get; set; }

    [CliOption("--firewall-rule-name")]
    public string? FirewallRuleName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--start-ip-address")]
    public string? StartIpAddress { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
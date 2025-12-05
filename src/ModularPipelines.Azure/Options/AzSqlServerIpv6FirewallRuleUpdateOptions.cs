using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server", "ipv6-firewall-rule", "update")]
public record AzSqlServerIpv6FirewallRuleUpdateOptions : AzOptions
{
    [CliOption("--end-ipv6-address")]
    public string? EndIpv6Address { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--start-ipv6-address")]
    public string? StartIpv6Address { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
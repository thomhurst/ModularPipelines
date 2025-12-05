using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server", "firewall-rule", "update")]
public record AzSqlServerFirewallRuleUpdateOptions : AzOptions
{
    [CliOption("--end-ip-address")]
    public string? EndIpAddress { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--start-ip-address")]
    public string? StartIpAddress { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
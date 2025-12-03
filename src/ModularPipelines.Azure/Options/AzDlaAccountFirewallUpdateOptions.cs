using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dla", "account", "firewall", "update")]
public record AzDlaAccountFirewallUpdateOptions(
[property: CliOption("--firewall-rule-name")] string FirewallRuleName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--end-ip-address")]
    public string? EndIpAddress { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--start-ip-address")]
    public string? StartIpAddress { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
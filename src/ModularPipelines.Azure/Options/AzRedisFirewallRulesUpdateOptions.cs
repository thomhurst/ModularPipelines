using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "firewall-rules", "update")]
public record AzRedisFirewallRulesUpdateOptions(
[property: CliOption("--end-ip")] string EndIp,
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--start-ip")] string StartIp
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
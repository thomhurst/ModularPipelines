using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "flexible-server", "firewall-rule", "create")]
public record AzPostgresFlexibleServerFirewallRuleCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--end-ip-address")]
    public string? EndIpAddress { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--start-ip-address")]
    public string? StartIpAddress { get; set; }
}
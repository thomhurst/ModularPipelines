using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server", "ipv6-firewall-rule", "create")]
public record AzSqlServerIpv6FirewallRuleCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server")] string Server
) : AzOptions
{
    [CliOption("--end-ipv6-address")]
    public string? EndIpv6Address { get; set; }

    [CliOption("--start-ipv6-address")]
    public string? StartIpv6Address { get; set; }
}
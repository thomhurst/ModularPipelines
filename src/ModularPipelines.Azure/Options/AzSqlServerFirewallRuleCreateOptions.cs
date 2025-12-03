using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "server", "firewall-rule", "create")]
public record AzSqlServerFirewallRuleCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server")] string Server
) : AzOptions
{
    [CliOption("--end-ip-address")]
    public string? EndIpAddress { get; set; }

    [CliOption("--start-ip-address")]
    public string? StartIpAddress { get; set; }
}
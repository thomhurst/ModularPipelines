using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "ipv6-firewall-rule", "create")]
public record AzSqlServerIpv6FirewallRuleCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server")] string Server
) : AzOptions
{
    [CommandSwitch("--end-ipv6-address")]
    public string? EndIpv6Address { get; set; }

    [CommandSwitch("--start-ipv6-address")]
    public string? StartIpv6Address { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "outbound-firewall-rule", "create")]
public record AzSqlServerOutboundFirewallRuleCreateOptions(
[property: CommandSwitch("--outbound-rule-fqdn")] string OutboundRuleFqdn,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server")] string Server
) : AzOptions
{
}
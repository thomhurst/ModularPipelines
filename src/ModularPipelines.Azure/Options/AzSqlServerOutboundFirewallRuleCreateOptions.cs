using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server", "outbound-firewall-rule", "create")]
public record AzSqlServerOutboundFirewallRuleCreateOptions(
[property: CliOption("--outbound-rule-fqdn")] string OutboundRuleFqdn,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server")] string Server
) : AzOptions;
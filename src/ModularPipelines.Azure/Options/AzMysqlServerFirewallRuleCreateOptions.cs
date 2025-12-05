using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "server", "firewall-rule", "create")]
public record AzMysqlServerFirewallRuleCreateOptions(
[property: CliOption("--end-ip-address")] string EndIpAddress,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName,
[property: CliOption("--start-ip-address")] string StartIpAddress
) : AzOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "server", "firewall-rule", "create")]
public record AzMysqlServerFirewallRuleCreateOptions(
[property: CommandSwitch("--end-ip-address")] string EndIpAddress,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server-name")] string ServerName,
[property: CommandSwitch("--start-ip-address")] string StartIpAddress
) : AzOptions;
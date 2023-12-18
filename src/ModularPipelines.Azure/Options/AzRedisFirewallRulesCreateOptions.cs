using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "firewall-rules", "create")]
public record AzRedisFirewallRulesCreateOptions(
[property: CommandSwitch("--end-ip")] string EndIp,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--start-ip")] string StartIp
) : AzOptions;
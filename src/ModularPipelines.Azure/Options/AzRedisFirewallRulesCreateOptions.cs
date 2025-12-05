using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("redis", "firewall-rules", "create")]
public record AzRedisFirewallRulesCreateOptions(
[property: CliOption("--end-ip")] string EndIp,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--start-ip")] string StartIp
) : AzOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "firewall-rules", "update")]
public record AzRedisFirewallRulesUpdateOptions(
[property: CommandSwitch("--end-ip")] string EndIp,
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--start-ip")] string StartIp
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}
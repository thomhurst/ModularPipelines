using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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
    public string? Subscription { get; set; }
}
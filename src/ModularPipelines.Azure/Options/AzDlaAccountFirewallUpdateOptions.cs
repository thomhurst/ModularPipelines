using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "account", "firewall", "update")]
public record AzDlaAccountFirewallUpdateOptions(
[property: CommandSwitch("--firewall-rule-name")] string FirewallRuleName
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--end-ip-address")]
    public string? EndIpAddress { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--start-ip-address")]
    public string? StartIpAddress { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}
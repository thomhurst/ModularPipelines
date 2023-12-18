using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dls", "account", "firewall", "update")]
public record AzDlsAccountFirewallUpdateOptions(
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
    public string? Subscription { get; set; }
}
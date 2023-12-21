using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "firewall-rule", "update")]
public record AzSqlServerFirewallRuleUpdateOptions : AzOptions
{
    [CommandSwitch("--end-ip-address")]
    public string? EndIpAddress { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [CommandSwitch("--start-ip-address")]
    public string? StartIpAddress { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}
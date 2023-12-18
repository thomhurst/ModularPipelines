using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw", "firewall", "save-log-profile")]
public record AzPaloAltoCloudngfwFirewallSaveLogProfileOptions : AzOptions
{
    [CommandSwitch("--application-insights")]
    public string? ApplicationInsights { get; set; }

    [CommandSwitch("--common-destination")]
    public string? CommonDestination { get; set; }

    [CommandSwitch("--decrypt-destination")]
    public string? DecryptDestination { get; set; }

    [CommandSwitch("--firewall-name")]
    public string? FirewallName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--log-option")]
    public string? LogOption { get; set; }

    [CommandSwitch("--log-type")]
    public string? LogType { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--threat-destination")]
    public string? ThreatDestination { get; set; }

    [CommandSwitch("--traffic-destination")]
    public string? TrafficDestination { get; set; }
}
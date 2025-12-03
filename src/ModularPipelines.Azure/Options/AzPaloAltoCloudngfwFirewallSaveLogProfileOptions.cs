using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("palo-alto", "cloudngfw", "firewall", "save-log-profile")]
public record AzPaloAltoCloudngfwFirewallSaveLogProfileOptions : AzOptions
{
    [CliOption("--application-insights")]
    public string? ApplicationInsights { get; set; }

    [CliOption("--common-destination")]
    public string? CommonDestination { get; set; }

    [CliOption("--decrypt-destination")]
    public string? DecryptDestination { get; set; }

    [CliOption("--firewall-name")]
    public string? FirewallName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--log-option")]
    public string? LogOption { get; set; }

    [CliOption("--log-type")]
    public string? LogType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--threat-destination")]
    public string? ThreatDestination { get; set; }

    [CliOption("--traffic-destination")]
    public string? TrafficDestination { get; set; }
}
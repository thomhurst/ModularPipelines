using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dynatrace", "monitor", "sso-config", "create")]
public record AzDynatraceMonitorSsoConfigCreateOptions(
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-domains")]
    public string? AadDomains { get; set; }

    [CliOption("--enterprise-app-id")]
    public string? EnterpriseAppId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--single-sign-on-state")]
    public string? SingleSignOnState { get; set; }

    [CliOption("--single-sign-on-url")]
    public string? SingleSignOnUrl { get; set; }
}
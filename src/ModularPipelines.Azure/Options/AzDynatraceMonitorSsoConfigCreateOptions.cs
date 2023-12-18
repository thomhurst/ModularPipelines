using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynatrace", "monitor", "sso-config", "create")]
public record AzDynatraceMonitorSsoConfigCreateOptions(
[property: CommandSwitch("--configuration-name")] string ConfigurationName,
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aad-domains")]
    public string? AadDomains { get; set; }

    [CommandSwitch("--enterprise-app-id")]
    public string? EnterpriseAppId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--single-sign-on-state")]
    public string? SingleSignOnState { get; set; }

    [CommandSwitch("--single-sign-on-url")]
    public string? SingleSignOnUrl { get; set; }
}
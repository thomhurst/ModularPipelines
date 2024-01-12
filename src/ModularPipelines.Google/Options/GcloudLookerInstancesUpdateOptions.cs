using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("looker", "instances", "update")]
public record GcloudLookerInstancesUpdateOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--allowed-email-domains")]
    public string[]? AllowedEmailDomains { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--custom-domain")]
    public string? CustomDomain { get; set; }

    [BooleanCommandSwitch("--public-ip-enabled")]
    public bool? PublicIpEnabled { get; set; }

    [CommandSwitch("--add-developer-users")]
    public string? AddDeveloperUsers { get; set; }

    [CommandSwitch("--add-standard-users")]
    public string? AddStandardUsers { get; set; }

    [CommandSwitch("--add-viewer-users")]
    public string? AddViewerUsers { get; set; }

    [CommandSwitch("--deny-maintenance-period-end-date")]
    public string? DenyMaintenancePeriodEndDate { get; set; }

    [CommandSwitch("--deny-maintenance-period-start-date")]
    public string? DenyMaintenancePeriodStartDate { get; set; }

    [CommandSwitch("--deny-maintenance-period-time")]
    public string? DenyMaintenancePeriodTime { get; set; }

    [CommandSwitch("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CommandSwitch("--maintenance-window-time")]
    public string? MaintenanceWindowTime { get; set; }

    [CommandSwitch("--oauth-client-id")]
    public string? OauthClientId { get; set; }

    [CommandSwitch("--oauth-client-secret")]
    public string? OauthClientSecret { get; set; }
}
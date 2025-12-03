using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("looker", "instances", "update")]
public record GcloudLookerInstancesUpdateOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--allowed-email-domains")]
    public string[]? AllowedEmailDomains { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--custom-domain")]
    public string? CustomDomain { get; set; }

    [CliFlag("--public-ip-enabled")]
    public bool? PublicIpEnabled { get; set; }

    [CliOption("--add-developer-users")]
    public string? AddDeveloperUsers { get; set; }

    [CliOption("--add-standard-users")]
    public string? AddStandardUsers { get; set; }

    [CliOption("--add-viewer-users")]
    public string? AddViewerUsers { get; set; }

    [CliOption("--deny-maintenance-period-end-date")]
    public string? DenyMaintenancePeriodEndDate { get; set; }

    [CliOption("--deny-maintenance-period-start-date")]
    public string? DenyMaintenancePeriodStartDate { get; set; }

    [CliOption("--deny-maintenance-period-time")]
    public string? DenyMaintenancePeriodTime { get; set; }

    [CliOption("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CliOption("--maintenance-window-time")]
    public string? MaintenanceWindowTime { get; set; }

    [CliOption("--oauth-client-id")]
    public string? OauthClientId { get; set; }

    [CliOption("--oauth-client-secret")]
    public string? OauthClientSecret { get; set; }
}
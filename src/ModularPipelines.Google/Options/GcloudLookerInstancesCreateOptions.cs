using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("looker", "instances", "create")]
public record GcloudLookerInstancesCreateOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region,
[property: CliOption("--edition")] string Edition,
[property: CliOption("--oauth-client-id")] string OauthClientId,
[property: CliOption("--oauth-client-secret")] string OauthClientSecret
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliFlag("--public-ip-enabled")]
    public bool? PublicIpEnabled { get; set; }

    [CliOption("--consumer-network")]
    public string? ConsumerNetwork { get; set; }

    [CliFlag("--private-ip-enabled")]
    public bool? PrivateIpEnabled { get; set; }

    [CliOption("--reserved-range")]
    public string? ReservedRange { get; set; }

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
}
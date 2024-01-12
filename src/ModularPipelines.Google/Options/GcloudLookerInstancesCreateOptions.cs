using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("looker", "instances", "create")]
public record GcloudLookerInstancesCreateOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--edition")] string Edition,
[property: CommandSwitch("--oauth-client-id")] string OauthClientId,
[property: CommandSwitch("--oauth-client-secret")] string OauthClientSecret
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [BooleanCommandSwitch("--public-ip-enabled")]
    public bool? PublicIpEnabled { get; set; }

    [CommandSwitch("--consumer-network")]
    public string? ConsumerNetwork { get; set; }

    [BooleanCommandSwitch("--private-ip-enabled")]
    public bool? PrivateIpEnabled { get; set; }

    [CommandSwitch("--reserved-range")]
    public string? ReservedRange { get; set; }

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
}
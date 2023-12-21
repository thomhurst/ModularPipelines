using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guestconfig", "guest-configuration-hcrp-assignment", "update")]
public record AzGuestconfigGuestConfigurationHcrpAssignmentUpdateOptions : AzOptions
{
    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--guest-configuration-assignment-name")]
    public string? GuestConfigurationAssignmentName { get; set; }

    [CommandSwitch("--guest-configuration-configuration-parameter")]
    public string? GuestConfigurationConfigurationParameter { get; set; }

    [CommandSwitch("--guest-configuration-configuration-setting")]
    public string? GuestConfigurationConfigurationSetting { get; set; }

    [CommandSwitch("--guest-configuration-name")]
    public string? GuestConfigurationName { get; set; }

    [CommandSwitch("--guest-configuration-version")]
    public string? GuestConfigurationVersion { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--latest-assignment-report-assignment")]
    public string? LatestAssignmentReportAssignment { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--machine-name")]
    public string? MachineName { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}
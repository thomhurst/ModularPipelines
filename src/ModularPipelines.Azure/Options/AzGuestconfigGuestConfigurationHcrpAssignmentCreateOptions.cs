using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guestconfig", "guest-configuration-hcrp-assignment", "create")]
public record AzGuestconfigGuestConfigurationHcrpAssignmentCreateOptions(
[property: CommandSwitch("--guest-configuration-assignment-name")] string GuestConfigurationAssignmentName,
[property: CommandSwitch("--machine-name")] string MachineName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--guest-configuration-configuration-parameter")]
    public string? GuestConfigurationConfigurationParameter { get; set; }

    [CommandSwitch("--guest-configuration-configuration-setting")]
    public string? GuestConfigurationConfigurationSetting { get; set; }

    [CommandSwitch("--guest-configuration-name")]
    public string? GuestConfigurationName { get; set; }

    [CommandSwitch("--guest-configuration-version")]
    public string? GuestConfigurationVersion { get; set; }

    [CommandSwitch("--latest-assignment-report-assignment")]
    public string? LatestAssignmentReportAssignment { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}
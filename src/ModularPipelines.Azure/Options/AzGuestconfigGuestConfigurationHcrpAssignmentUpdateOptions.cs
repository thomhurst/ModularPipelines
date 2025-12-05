using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("guestconfig", "guest-configuration-hcrp-assignment", "update")]
public record AzGuestconfigGuestConfigurationHcrpAssignmentUpdateOptions : AzOptions
{
    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--guest-configuration-assignment-name")]
    public string? GuestConfigurationAssignmentName { get; set; }

    [CliOption("--guest-configuration-configuration-parameter")]
    public string? GuestConfigurationConfigurationParameter { get; set; }

    [CliOption("--guest-configuration-configuration-setting")]
    public string? GuestConfigurationConfigurationSetting { get; set; }

    [CliOption("--guest-configuration-name")]
    public string? GuestConfigurationName { get; set; }

    [CliOption("--guest-configuration-version")]
    public string? GuestConfigurationVersion { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--latest-assignment-report-assignment")]
    public string? LatestAssignmentReportAssignment { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--machine-name")]
    public string? MachineName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
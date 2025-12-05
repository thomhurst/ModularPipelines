using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("guestconfig", "guest-configuration-assignment", "create")]
public record AzGuestconfigGuestConfigurationAssignmentCreateOptions(
[property: CliOption("--guest-configuration-assignment-name")] string GuestConfigurationAssignmentName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--guest-configuration-configuration-parameter")]
    public string? GuestConfigurationConfigurationParameter { get; set; }

    [CliOption("--guest-configuration-configuration-setting")]
    public string? GuestConfigurationConfigurationSetting { get; set; }

    [CliOption("--guest-configuration-name")]
    public string? GuestConfigurationName { get; set; }

    [CliOption("--guest-configuration-version")]
    public string? GuestConfigurationVersion { get; set; }

    [CliOption("--latest-assignment-report-assignment")]
    public string? LatestAssignmentReportAssignment { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}
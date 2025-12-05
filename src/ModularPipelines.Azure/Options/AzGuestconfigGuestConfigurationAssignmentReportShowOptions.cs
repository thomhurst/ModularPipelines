using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("guestconfig", "guest-configuration-assignment-report", "show")]
public record AzGuestconfigGuestConfigurationAssignmentReportShowOptions : AzOptions
{
    [CliOption("--guest-configuration-assignment-name")]
    public string? GuestConfigurationAssignmentName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--report-id")]
    public string? ReportId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vm-name")]
    public string? VmName { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("guestconfig", "guest-configuration-hcrp-assignment-report", "show")]
public record AzGuestconfigGuestConfigurationHcrpAssignmentReportShowOptions : AzOptions
{
    [CliOption("--guest-configuration-assignment-name")]
    public string? GuestConfigurationAssignmentName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--machine-name")]
    public string? MachineName { get; set; }

    [CliOption("--report-id")]
    public string? ReportId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guestconfig", "guest-configuration-hcrp-assignment-report", "show")]
public record AzGuestconfigGuestConfigurationHcrpAssignmentReportShowOptions : AzOptions
{
    [CommandSwitch("--guest-configuration-assignment-name")]
    public string? GuestConfigurationAssignmentName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--machine-name")]
    public string? MachineName { get; set; }

    [CommandSwitch("--report-id")]
    public string? ReportId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}
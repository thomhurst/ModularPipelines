using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guestconfig", "guest-configuration-hcrp-assignment-report", "list")]
public record AzGuestconfigGuestConfigurationHcrpAssignmentReportListOptions(
[property: CommandSwitch("--guest-configuration-assignment-name")] string GuestConfigurationAssignmentName,
[property: CommandSwitch("--machine-name")] string MachineName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--report-id")]
    public string? ReportId { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}
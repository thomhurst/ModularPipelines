using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guestconfig", "guest-configuration-assignment-report", "list")]
public record AzGuestconfigGuestConfigurationAssignmentReportListOptions(
[property: CommandSwitch("--guest-configuration-assignment-name")] string GuestConfigurationAssignmentName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--report-id")]
    public string? ReportId { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}
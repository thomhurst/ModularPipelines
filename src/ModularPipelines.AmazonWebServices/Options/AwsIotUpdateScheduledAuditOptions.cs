using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-scheduled-audit")]
public record AwsIotUpdateScheduledAuditOptions(
[property: CommandSwitch("--scheduled-audit-name")] string ScheduledAuditName
) : AwsOptions
{
    [CommandSwitch("--frequency")]
    public string? Frequency { get; set; }

    [CommandSwitch("--day-of-month")]
    public string? DayOfMonth { get; set; }

    [CommandSwitch("--day-of-week")]
    public string? DayOfWeek { get; set; }

    [CommandSwitch("--target-check-names")]
    public string[]? TargetCheckNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
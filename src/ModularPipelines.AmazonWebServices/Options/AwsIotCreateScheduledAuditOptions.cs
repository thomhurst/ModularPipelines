using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-scheduled-audit")]
public record AwsIotCreateScheduledAuditOptions(
[property: CommandSwitch("--frequency")] string Frequency,
[property: CommandSwitch("--target-check-names")] string[] TargetCheckNames,
[property: CommandSwitch("--scheduled-audit-name")] string ScheduledAuditName
) : AwsOptions
{
    [CommandSwitch("--day-of-month")]
    public string? DayOfMonth { get; set; }

    [CommandSwitch("--day-of-week")]
    public string? DayOfWeek { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
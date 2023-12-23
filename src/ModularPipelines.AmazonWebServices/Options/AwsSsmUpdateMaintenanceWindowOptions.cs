using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-maintenance-window")]
public record AwsSsmUpdateMaintenanceWindowOptions(
[property: CommandSwitch("--window-id")] string WindowId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--schedule-timezone")]
    public string? ScheduleTimezone { get; set; }

    [CommandSwitch("--schedule-offset")]
    public int? ScheduleOffset { get; set; }

    [CommandSwitch("--duration")]
    public int? Duration { get; set; }

    [CommandSwitch("--cutoff")]
    public int? Cutoff { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "create-maintenance-window")]
public record AwsSsmCreateMaintenanceWindowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--schedule")] string Schedule,
[property: CommandSwitch("--duration")] int Duration,
[property: CommandSwitch("--cutoff")] int Cutoff
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--schedule-timezone")]
    public string? ScheduleTimezone { get; set; }

    [CommandSwitch("--schedule-offset")]
    public int? ScheduleOffset { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "put-scheduled-update-group-action")]
public record AwsAutoscalingPutScheduledUpdateGroupActionOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CommandSwitch("--scheduled-action-name")] string ScheduledActionName
) : AwsOptions
{
    [CommandSwitch("--time")]
    public long? Time { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--recurrence")]
    public string? Recurrence { get; set; }

    [CommandSwitch("--min-size")]
    public int? MinSize { get; set; }

    [CommandSwitch("--max-size")]
    public int? MaxSize { get; set; }

    [CommandSwitch("--desired-capacity")]
    public int? DesiredCapacity { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
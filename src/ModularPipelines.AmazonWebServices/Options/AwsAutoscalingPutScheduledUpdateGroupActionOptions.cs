using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "put-scheduled-update-group-action")]
public record AwsAutoscalingPutScheduledUpdateGroupActionOptions(
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CliOption("--scheduled-action-name")] string ScheduledActionName
) : AwsOptions
{
    [CliOption("--time")]
    public long? Time { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--recurrence")]
    public string? Recurrence { get; set; }

    [CliOption("--min-size")]
    public int? MinSize { get; set; }

    [CliOption("--max-size")]
    public int? MaxSize { get; set; }

    [CliOption("--desired-capacity")]
    public int? DesiredCapacity { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
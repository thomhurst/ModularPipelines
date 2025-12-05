using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "describe-scheduled-actions")]
public record AwsAutoscalingDescribeScheduledActionsOptions : AwsOptions
{
    [CliOption("--auto-scaling-group-name")]
    public string? AutoScalingGroupName { get; set; }

    [CliOption("--scheduled-action-names")]
    public string[]? ScheduledActionNames { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "describe-scaling-activities")]
public record AwsAutoscalingDescribeScalingActivitiesOptions : AwsOptions
{
    [CommandSwitch("--activity-ids")]
    public string[]? ActivityIds { get; set; }

    [CommandSwitch("--auto-scaling-group-name")]
    public string? AutoScalingGroupName { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
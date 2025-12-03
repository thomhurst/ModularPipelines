using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "describe-scaling-activities")]
public record AwsAutoscalingDescribeScalingActivitiesOptions : AwsOptions
{
    [CliOption("--activity-ids")]
    public string[]? ActivityIds { get; set; }

    [CliOption("--auto-scaling-group-name")]
    public string? AutoScalingGroupName { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
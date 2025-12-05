using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "cancel-instance-refresh")]
public record AwsAutoscalingCancelInstanceRefreshOptions(
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
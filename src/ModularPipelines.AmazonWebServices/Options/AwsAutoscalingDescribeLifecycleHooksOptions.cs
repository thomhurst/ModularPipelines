using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "describe-lifecycle-hooks")]
public record AwsAutoscalingDescribeLifecycleHooksOptions(
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CliOption("--lifecycle-hook-names")]
    public string[]? LifecycleHookNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "put-warm-pool")]
public record AwsAutoscalingPutWarmPoolOptions(
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CliOption("--max-group-prepared-capacity")]
    public int? MaxGroupPreparedCapacity { get; set; }

    [CliOption("--min-size")]
    public int? MinSize { get; set; }

    [CliOption("--pool-state")]
    public string? PoolState { get; set; }

    [CliOption("--instance-reuse-policy")]
    public string? InstanceReusePolicy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
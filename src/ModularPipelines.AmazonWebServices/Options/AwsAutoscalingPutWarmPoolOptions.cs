using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "put-warm-pool")]
public record AwsAutoscalingPutWarmPoolOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CommandSwitch("--max-group-prepared-capacity")]
    public int? MaxGroupPreparedCapacity { get; set; }

    [CommandSwitch("--min-size")]
    public int? MinSize { get; set; }

    [CommandSwitch("--pool-state")]
    public string? PoolState { get; set; }

    [CommandSwitch("--instance-reuse-policy")]
    public string? InstanceReusePolicy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
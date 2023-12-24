using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "execute-policy")]
public record AwsAutoscalingExecutePolicyOptions(
[property: CommandSwitch("--policy-name")] string PolicyName
) : AwsOptions
{
    [CommandSwitch("--auto-scaling-group-name")]
    public string? AutoScalingGroupName { get; set; }

    [CommandSwitch("--metric-value")]
    public double? MetricValue { get; set; }

    [CommandSwitch("--breach-threshold")]
    public double? BreachThreshold { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
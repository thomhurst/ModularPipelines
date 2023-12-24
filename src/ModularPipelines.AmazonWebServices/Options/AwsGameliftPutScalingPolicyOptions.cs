using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "put-scaling-policy")]
public record AwsGameliftPutScalingPolicyOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--fleet-id")] string FleetId,
[property: CommandSwitch("--metric-name")] string MetricName
) : AwsOptions
{
    [CommandSwitch("--scaling-adjustment")]
    public int? ScalingAdjustment { get; set; }

    [CommandSwitch("--scaling-adjustment-type")]
    public string? ScalingAdjustmentType { get; set; }

    [CommandSwitch("--threshold")]
    public double? Threshold { get; set; }

    [CommandSwitch("--comparison-operator")]
    public string? ComparisonOperator { get; set; }

    [CommandSwitch("--evaluation-periods")]
    public int? EvaluationPeriods { get; set; }

    [CommandSwitch("--policy-type")]
    public string? PolicyType { get; set; }

    [CommandSwitch("--target-configuration")]
    public string? TargetConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
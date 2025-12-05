using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "put-scaling-policy")]
public record AwsGameliftPutScalingPolicyOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--fleet-id")] string FleetId,
[property: CliOption("--metric-name")] string MetricName
) : AwsOptions
{
    [CliOption("--scaling-adjustment")]
    public int? ScalingAdjustment { get; set; }

    [CliOption("--scaling-adjustment-type")]
    public string? ScalingAdjustmentType { get; set; }

    [CliOption("--threshold")]
    public double? Threshold { get; set; }

    [CliOption("--comparison-operator")]
    public string? ComparisonOperator { get; set; }

    [CliOption("--evaluation-periods")]
    public int? EvaluationPeriods { get; set; }

    [CliOption("--policy-type")]
    public string? PolicyType { get; set; }

    [CliOption("--target-configuration")]
    public string? TargetConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
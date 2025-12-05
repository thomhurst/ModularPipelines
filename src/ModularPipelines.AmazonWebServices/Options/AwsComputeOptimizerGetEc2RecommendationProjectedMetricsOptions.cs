using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute-optimizer", "get-ec2-recommendation-projected-metrics")]
public record AwsComputeOptimizerGetEc2RecommendationProjectedMetricsOptions(
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--stat")] string Stat,
[property: CliOption("--period")] int Period,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime
) : AwsOptions
{
    [CliOption("--recommendation-preferences")]
    public string? RecommendationPreferences { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
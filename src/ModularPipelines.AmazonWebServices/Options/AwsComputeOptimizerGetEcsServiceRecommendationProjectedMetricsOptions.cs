using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute-optimizer", "get-ecs-service-recommendation-projected-metrics")]
public record AwsComputeOptimizerGetEcsServiceRecommendationProjectedMetricsOptions(
[property: CliOption("--service-arn")] string ServiceArn,
[property: CliOption("--stat")] string Stat,
[property: CliOption("--period")] int Period,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
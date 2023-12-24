using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute-optimizer", "get-ecs-service-recommendation-projected-metrics")]
public record AwsComputeOptimizerGetEcsServiceRecommendationProjectedMetricsOptions(
[property: CommandSwitch("--service-arn")] string ServiceArn,
[property: CommandSwitch("--stat")] string Stat,
[property: CommandSwitch("--period")] int Period,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling-plans", "get-scaling-plan-resource-forecast-data")]
public record AwsAutoscalingPlansGetScalingPlanResourceForecastDataOptions(
[property: CliOption("--scaling-plan-name")] string ScalingPlanName,
[property: CliOption("--scaling-plan-version")] long ScalingPlanVersion,
[property: CliOption("--service-namespace")] string ServiceNamespace,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--scalable-dimension")] string ScalableDimension,
[property: CliOption("--forecast-data-type")] string ForecastDataType,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
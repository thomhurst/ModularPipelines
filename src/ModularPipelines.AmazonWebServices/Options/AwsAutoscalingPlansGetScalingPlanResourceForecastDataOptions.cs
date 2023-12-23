using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling-plans", "get-scaling-plan-resource-forecast-data")]
public record AwsAutoscalingPlansGetScalingPlanResourceForecastDataOptions(
[property: CommandSwitch("--scaling-plan-name")] string ScalingPlanName,
[property: CommandSwitch("--scaling-plan-version")] long ScalingPlanVersion,
[property: CommandSwitch("--service-namespace")] string ServiceNamespace,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--scalable-dimension")] string ScalableDimension,
[property: CommandSwitch("--forecast-data-type")] string ForecastDataType,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
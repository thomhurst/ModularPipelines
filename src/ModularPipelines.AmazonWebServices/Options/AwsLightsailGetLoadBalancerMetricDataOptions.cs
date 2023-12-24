using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-load-balancer-metric-data")]
public record AwsLightsailGetLoadBalancerMetricDataOptions(
[property: CommandSwitch("--load-balancer-name")] string LoadBalancerName,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--period")] int Period,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--unit")] string Unit,
[property: CommandSwitch("--statistics")] string[] Statistics
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
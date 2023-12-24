using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-distribution-metric-data")]
public record AwsLightsailGetDistributionMetricDataOptions(
[property: CommandSwitch("--distribution-name")] string DistributionName,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--period")] int Period,
[property: CommandSwitch("--unit")] string Unit,
[property: CommandSwitch("--statistics")] string[] Statistics
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
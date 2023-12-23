using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-container-service-metric-data")]
public record AwsLightsailGetContainerServiceMetricDataOptions(
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--period")] int Period,
[property: CommandSwitch("--statistics")] string[] Statistics
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
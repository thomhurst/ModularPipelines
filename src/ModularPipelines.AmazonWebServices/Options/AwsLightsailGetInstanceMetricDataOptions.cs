using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-instance-metric-data")]
public record AwsLightsailGetInstanceMetricDataOptions(
[property: CommandSwitch("--instance-name")] string InstanceName,
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
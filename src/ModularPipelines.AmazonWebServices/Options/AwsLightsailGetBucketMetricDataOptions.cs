using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-bucket-metric-data")]
public record AwsLightsailGetBucketMetricDataOptions(
[property: CommandSwitch("--bucket-name")] string BucketName,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--period")] int Period,
[property: CommandSwitch("--statistics")] string[] Statistics,
[property: CommandSwitch("--unit")] string Unit
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
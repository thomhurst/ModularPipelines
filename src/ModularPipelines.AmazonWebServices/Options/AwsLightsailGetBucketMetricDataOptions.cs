using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-bucket-metric-data")]
public record AwsLightsailGetBucketMetricDataOptions(
[property: CliOption("--bucket-name")] string BucketName,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--period")] int Period,
[property: CliOption("--statistics")] string[] Statistics,
[property: CliOption("--unit")] string Unit
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
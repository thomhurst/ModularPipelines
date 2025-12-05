using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-distribution-metric-data")]
public record AwsLightsailGetDistributionMetricDataOptions(
[property: CliOption("--distribution-name")] string DistributionName,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--period")] int Period,
[property: CliOption("--unit")] string Unit,
[property: CliOption("--statistics")] string[] Statistics
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
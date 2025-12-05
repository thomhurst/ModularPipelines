using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-container-service-metric-data")]
public record AwsLightsailGetContainerServiceMetricDataOptions(
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--period")] int Period,
[property: CliOption("--statistics")] string[] Statistics
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
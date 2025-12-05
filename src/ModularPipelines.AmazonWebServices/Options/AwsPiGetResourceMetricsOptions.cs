using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pi", "get-resource-metrics")]
public record AwsPiGetResourceMetricsOptions(
[property: CliOption("--service-type")] string ServiceType,
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--metric-queries")] string[] MetricQueries,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime
) : AwsOptions
{
    [CliOption("--period-in-seconds")]
    public int? PeriodInSeconds { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--period-alignment")]
    public string? PeriodAlignment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
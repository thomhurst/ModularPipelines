using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-cost-forecast")]
public record AwsCeGetCostForecastOptions(
[property: CliOption("--time-period")] string TimePeriod,
[property: CliOption("--metric")] string Metric,
[property: CliOption("--granularity")] string Granularity
) : AwsOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--prediction-interval-level")]
    public int? PredictionIntervalLevel { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
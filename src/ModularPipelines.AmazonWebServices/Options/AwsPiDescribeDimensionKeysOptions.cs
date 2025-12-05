using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pi", "describe-dimension-keys")]
public record AwsPiDescribeDimensionKeysOptions(
[property: CliOption("--service-type")] string ServiceType,
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--metric")] string Metric,
[property: CliOption("--group-by")] string GroupBy
) : AwsOptions
{
    [CliOption("--period-in-seconds")]
    public int? PeriodInSeconds { get; set; }

    [CliOption("--additional-metrics")]
    public string[]? AdditionalMetrics { get; set; }

    [CliOption("--partition-by")]
    public string? PartitionBy { get; set; }

    [CliOption("--filter")]
    public IEnumerable<KeyValue>? Filter { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
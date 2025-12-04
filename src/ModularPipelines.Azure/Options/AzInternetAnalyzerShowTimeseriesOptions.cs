using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("internet-analyzer", "show-timeseries")]
public record AzInternetAnalyzerShowTimeseriesOptions(
[property: CliOption("--aggregation-interval")] string AggregationInterval,
[property: CliOption("--end-date-time-utc")] string EndDateTimeUtc,
[property: CliOption("--endpoint")] string Endpoint,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--start-date-time-utc")] string StartDateTimeUtc,
[property: CliOption("--test-name")] string TestName,
[property: CliOption("--timeseries-type")] string TimeseriesType
) : AzOptions
{
    [CliOption("--country")]
    public int? Country { get; set; }
}
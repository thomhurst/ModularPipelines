using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("internet-analyzer", "show-timeseries")]
public record AzInternetAnalyzerShowTimeseriesOptions(
[property: CommandSwitch("--aggregation-interval")] string AggregationInterval,
[property: CommandSwitch("--end-date-time-utc")] string EndDateTimeUtc,
[property: CommandSwitch("--endpoint")] string Endpoint,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--start-date-time-utc")] string StartDateTimeUtc,
[property: CommandSwitch("--test-name")] string TestName,
[property: CommandSwitch("--timeseries-type")] string TimeseriesType
) : AzOptions
{
    [CommandSwitch("--country")]
    public int? Country { get; set; }
}
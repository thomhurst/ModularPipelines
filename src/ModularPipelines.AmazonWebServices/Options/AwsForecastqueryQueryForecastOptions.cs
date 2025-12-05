using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecastquery", "query-forecast")]
public record AwsForecastqueryQueryForecastOptions(
[property: CliOption("--forecast-arn")] string ForecastArn,
[property: CliOption("--filters")] IEnumerable<KeyValue> Filters
) : AwsOptions
{
    [CliOption("--start-date")]
    public string? StartDate { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
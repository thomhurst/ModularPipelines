using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecastquery", "query-forecast")]
public record AwsForecastqueryQueryForecastOptions(
[property: CommandSwitch("--forecast-arn")] string ForecastArn,
[property: CommandSwitch("--filters")] IEnumerable<KeyValue> Filters
) : AwsOptions
{
    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
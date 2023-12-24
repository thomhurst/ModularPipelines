using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecastquery", "query-what-if-forecast")]
public record AwsForecastqueryQueryWhatIfForecastOptions(
[property: CommandSwitch("--what-if-forecast-arn")] string WhatIfForecastArn,
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
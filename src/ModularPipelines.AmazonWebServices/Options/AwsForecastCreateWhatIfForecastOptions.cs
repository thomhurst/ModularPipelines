using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "create-what-if-forecast")]
public record AwsForecastCreateWhatIfForecastOptions(
[property: CommandSwitch("--what-if-forecast-name")] string WhatIfForecastName,
[property: CommandSwitch("--what-if-analysis-arn")] string WhatIfAnalysisArn
) : AwsOptions
{
    [CommandSwitch("--time-series-transformations")]
    public string[]? TimeSeriesTransformations { get; set; }

    [CommandSwitch("--time-series-replacements-data-source")]
    public string? TimeSeriesReplacementsDataSource { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
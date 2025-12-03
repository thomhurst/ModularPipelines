using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-what-if-forecast")]
public record AwsForecastCreateWhatIfForecastOptions(
[property: CliOption("--what-if-forecast-name")] string WhatIfForecastName,
[property: CliOption("--what-if-analysis-arn")] string WhatIfAnalysisArn
) : AwsOptions
{
    [CliOption("--time-series-transformations")]
    public string[]? TimeSeriesTransformations { get; set; }

    [CliOption("--time-series-replacements-data-source")]
    public string? TimeSeriesReplacementsDataSource { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
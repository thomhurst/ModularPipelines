using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-what-if-analysis")]
public record AwsForecastCreateWhatIfAnalysisOptions(
[property: CliOption("--what-if-analysis-name")] string WhatIfAnalysisName,
[property: CliOption("--forecast-arn")] string ForecastArn
) : AwsOptions
{
    [CliOption("--time-series-selector")]
    public string? TimeSeriesSelector { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
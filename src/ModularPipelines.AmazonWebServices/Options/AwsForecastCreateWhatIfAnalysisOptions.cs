using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "create-what-if-analysis")]
public record AwsForecastCreateWhatIfAnalysisOptions(
[property: CommandSwitch("--what-if-analysis-name")] string WhatIfAnalysisName,
[property: CommandSwitch("--forecast-arn")] string ForecastArn
) : AwsOptions
{
    [CommandSwitch("--time-series-selector")]
    public string? TimeSeriesSelector { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
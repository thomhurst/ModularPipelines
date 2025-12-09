using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "delete-what-if-analysis")]
public record AwsForecastDeleteWhatIfAnalysisOptions(
[property: CliOption("--what-if-analysis-arn")] string WhatIfAnalysisArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
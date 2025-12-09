using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "describe-what-if-analysis")]
public record AwsForecastDescribeWhatIfAnalysisOptions(
[property: CliOption("--what-if-analysis-arn")] string WhatIfAnalysisArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
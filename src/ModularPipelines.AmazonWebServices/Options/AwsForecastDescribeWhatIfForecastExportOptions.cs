using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "describe-what-if-forecast-export")]
public record AwsForecastDescribeWhatIfForecastExportOptions(
[property: CliOption("--what-if-forecast-export-arn")] string WhatIfForecastExportArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
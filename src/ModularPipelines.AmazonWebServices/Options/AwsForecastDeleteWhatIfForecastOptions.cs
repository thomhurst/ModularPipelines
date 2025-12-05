using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "delete-what-if-forecast")]
public record AwsForecastDeleteWhatIfForecastOptions(
[property: CliOption("--what-if-forecast-arn")] string WhatIfForecastArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
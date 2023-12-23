using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "describe-what-if-forecast-export")]
public record AwsForecastDescribeWhatIfForecastExportOptions(
[property: CommandSwitch("--what-if-forecast-export-arn")] string WhatIfForecastExportArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
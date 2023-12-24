using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "delete-what-if-forecast-export")]
public record AwsForecastDeleteWhatIfForecastExportOptions(
[property: CommandSwitch("--what-if-forecast-export-arn")] string WhatIfForecastExportArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
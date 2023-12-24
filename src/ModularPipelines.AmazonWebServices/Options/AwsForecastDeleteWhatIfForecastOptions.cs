using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "delete-what-if-forecast")]
public record AwsForecastDeleteWhatIfForecastOptions(
[property: CommandSwitch("--what-if-forecast-arn")] string WhatIfForecastArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
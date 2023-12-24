using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "describe-what-if-forecast")]
public record AwsForecastDescribeWhatIfForecastOptions(
[property: CommandSwitch("--what-if-forecast-arn")] string WhatIfForecastArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
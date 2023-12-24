using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "delete-what-if-analysis")]
public record AwsForecastDeleteWhatIfAnalysisOptions(
[property: CommandSwitch("--what-if-analysis-arn")] string WhatIfAnalysisArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
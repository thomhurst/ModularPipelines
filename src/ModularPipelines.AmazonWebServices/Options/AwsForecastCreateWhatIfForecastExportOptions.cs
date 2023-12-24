using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "create-what-if-forecast-export")]
public record AwsForecastCreateWhatIfForecastExportOptions(
[property: CommandSwitch("--what-if-forecast-export-name")] string WhatIfForecastExportName,
[property: CommandSwitch("--what-if-forecast-arns")] string[] WhatIfForecastArns,
[property: CommandSwitch("--destination")] string Destination
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
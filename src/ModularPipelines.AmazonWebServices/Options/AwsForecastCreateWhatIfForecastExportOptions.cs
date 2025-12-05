using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-what-if-forecast-export")]
public record AwsForecastCreateWhatIfForecastExportOptions(
[property: CliOption("--what-if-forecast-export-name")] string WhatIfForecastExportName,
[property: CliOption("--what-if-forecast-arns")] string[] WhatIfForecastArns,
[property: CliOption("--destination")] string Destination
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
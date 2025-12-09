using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-forecast-export-job")]
public record AwsForecastCreateForecastExportJobOptions(
[property: CliOption("--forecast-export-job-name")] string ForecastExportJobName,
[property: CliOption("--forecast-arn")] string ForecastArn,
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
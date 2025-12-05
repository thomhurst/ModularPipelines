using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-dataset-import-job")]
public record AwsForecastCreateDatasetImportJobOptions(
[property: CliOption("--dataset-import-job-name")] string DatasetImportJobName,
[property: CliOption("--dataset-arn")] string DatasetArn,
[property: CliOption("--data-source")] string DataSource
) : AwsOptions
{
    [CliOption("--timestamp-format")]
    public string? TimestampFormat { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }

    [CliOption("--geolocation-format")]
    public string? GeolocationFormat { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--import-mode")]
    public string? ImportMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
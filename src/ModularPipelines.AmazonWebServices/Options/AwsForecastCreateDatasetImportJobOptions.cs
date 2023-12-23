using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "create-dataset-import-job")]
public record AwsForecastCreateDatasetImportJobOptions(
[property: CommandSwitch("--dataset-import-job-name")] string DatasetImportJobName,
[property: CommandSwitch("--dataset-arn")] string DatasetArn,
[property: CommandSwitch("--data-source")] string DataSource
) : AwsOptions
{
    [CommandSwitch("--timestamp-format")]
    public string? TimestampFormat { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }

    [CommandSwitch("--geolocation-format")]
    public string? GeolocationFormat { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--import-mode")]
    public string? ImportMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
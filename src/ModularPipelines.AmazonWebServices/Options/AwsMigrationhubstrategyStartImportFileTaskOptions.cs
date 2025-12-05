using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhubstrategy", "start-import-file-task")]
public record AwsMigrationhubstrategyStartImportFileTaskOptions(
[property: CliOption("--s3-bucket")] string S3Bucket,
[property: CliOption("--name")] string Name,
[property: CliOption("--s3key")] string S3key
) : AwsOptions
{
    [CliOption("--data-source-type")]
    public string? DataSourceType { get; set; }

    [CliOption("--group-id")]
    public string[]? GroupId { get; set; }

    [CliOption("--s3bucket-for-report-data")]
    public string? S3bucketForReportData { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
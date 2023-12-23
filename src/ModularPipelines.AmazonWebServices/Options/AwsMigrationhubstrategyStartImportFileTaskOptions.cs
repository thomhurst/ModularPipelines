using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhubstrategy", "start-import-file-task")]
public record AwsMigrationhubstrategyStartImportFileTaskOptions(
[property: CommandSwitch("--s3-bucket")] string S3Bucket,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--s3key")] string S3key
) : AwsOptions
{
    [CommandSwitch("--data-source-type")]
    public string? DataSourceType { get; set; }

    [CommandSwitch("--group-id")]
    public string[]? GroupId { get; set; }

    [CommandSwitch("--s3bucket-for-report-data")]
    public string? S3bucketForReportData { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
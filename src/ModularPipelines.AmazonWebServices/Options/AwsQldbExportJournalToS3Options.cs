using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qldb", "export-journal-to-s3")]
public record AwsQldbExportJournalToS3Options(
[property: CliOption("--name")] string Name,
[property: CliOption("--inclusive-start-time")] long InclusiveStartTime,
[property: CliOption("--exclusive-end-time")] long ExclusiveEndTime,
[property: CliOption("--s3-export-configuration")] string S3ExportConfiguration,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--output-format")]
    public string? OutputFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
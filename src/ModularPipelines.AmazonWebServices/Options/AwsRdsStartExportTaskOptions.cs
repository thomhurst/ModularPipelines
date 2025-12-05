using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "start-export-task")]
public record AwsRdsStartExportTaskOptions(
[property: CliOption("--export-task-identifier")] string ExportTaskIdentifier,
[property: CliOption("--source-arn")] string SourceArn,
[property: CliOption("--s3-bucket-name")] string S3BucketName,
[property: CliOption("--iam-role-arn")] string IamRoleArn,
[property: CliOption("--kms-key-id")] string KmsKeyId
) : AwsOptions
{
    [CliOption("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CliOption("--export-only")]
    public string[]? ExportOnly { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
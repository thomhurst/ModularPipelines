using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "start-export-task")]
public record AwsRdsStartExportTaskOptions(
[property: CommandSwitch("--export-task-identifier")] string ExportTaskIdentifier,
[property: CommandSwitch("--source-arn")] string SourceArn,
[property: CommandSwitch("--s3-bucket-name")] string S3BucketName,
[property: CommandSwitch("--iam-role-arn")] string IamRoleArn,
[property: CommandSwitch("--kms-key-id")] string KmsKeyId
) : AwsOptions
{
    [CommandSwitch("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CommandSwitch("--export-only")]
    public string[]? ExportOnly { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "export-table-to-point-in-time")]
public record AwsDynamodbExportTableToPointInTimeOptions(
[property: CommandSwitch("--table-arn")] string TableArn,
[property: CommandSwitch("--s3-bucket")] string S3Bucket
) : AwsOptions
{
    [CommandSwitch("--export-time")]
    public long? ExportTime { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--s3-bucket-owner")]
    public string? S3BucketOwner { get; set; }

    [CommandSwitch("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CommandSwitch("--s3-sse-algorithm")]
    public string? S3SseAlgorithm { get; set; }

    [CommandSwitch("--s3-sse-kms-key-id")]
    public string? S3SseKmsKeyId { get; set; }

    [CommandSwitch("--export-format")]
    public string? ExportFormat { get; set; }

    [CommandSwitch("--export-type")]
    public string? ExportType { get; set; }

    [CommandSwitch("--incremental-export-specification")]
    public string? IncrementalExportSpecification { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
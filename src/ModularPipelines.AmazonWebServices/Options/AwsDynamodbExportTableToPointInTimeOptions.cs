using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "export-table-to-point-in-time")]
public record AwsDynamodbExportTableToPointInTimeOptions(
[property: CliOption("--table-arn")] string TableArn,
[property: CliOption("--s3-bucket")] string S3Bucket
) : AwsOptions
{
    [CliOption("--export-time")]
    public long? ExportTime { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--s3-bucket-owner")]
    public string? S3BucketOwner { get; set; }

    [CliOption("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CliOption("--s3-sse-algorithm")]
    public string? S3SseAlgorithm { get; set; }

    [CliOption("--s3-sse-kms-key-id")]
    public string? S3SseKmsKeyId { get; set; }

    [CliOption("--export-format")]
    public string? ExportFormat { get; set; }

    [CliOption("--export-type")]
    public string? ExportType { get; set; }

    [CliOption("--incremental-export-specification")]
    public string? IncrementalExportSpecification { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
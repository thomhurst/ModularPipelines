using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "export-serverless-cache-snapshot")]
public record AwsElasticacheExportServerlessCacheSnapshotOptions(
[property: CliOption("--serverless-cache-snapshot-name")] string ServerlessCacheSnapshotName,
[property: CliOption("--s3-bucket-name")] string S3BucketName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
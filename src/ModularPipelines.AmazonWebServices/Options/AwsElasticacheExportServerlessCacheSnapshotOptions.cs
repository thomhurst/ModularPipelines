using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "export-serverless-cache-snapshot")]
public record AwsElasticacheExportServerlessCacheSnapshotOptions(
[property: CommandSwitch("--serverless-cache-snapshot-name")] string ServerlessCacheSnapshotName,
[property: CommandSwitch("--s3-bucket-name")] string S3BucketName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
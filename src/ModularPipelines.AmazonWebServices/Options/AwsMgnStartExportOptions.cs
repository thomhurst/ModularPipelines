using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "start-export")]
public record AwsMgnStartExportOptions(
[property: CliOption("--s3-bucket")] string S3Bucket,
[property: CliOption("--s3-key")] string S3Key
) : AwsOptions
{
    [CliOption("--s3-bucket-owner")]
    public string? S3BucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
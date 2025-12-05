using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "put-bucket-ownership-controls")]
public record AwsS3apiPutBucketOwnershipControlsOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--ownership-controls")] string OwnershipControls
) : AwsOptions
{
    [CliOption("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
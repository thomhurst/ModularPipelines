using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "put-bucket-lifecycle-configuration")]
public record AwsS3controlPutBucketLifecycleConfigurationOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--bucket")] string Bucket
) : AwsOptions
{
    [CliOption("--lifecycle-configuration")]
    public string? LifecycleConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
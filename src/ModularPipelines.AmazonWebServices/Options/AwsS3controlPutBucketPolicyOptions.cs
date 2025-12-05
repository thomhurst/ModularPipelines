using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "put-bucket-policy")]
public record AwsS3controlPutBucketPolicyOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--policy")] string Policy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
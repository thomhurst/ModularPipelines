using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "get-bucket-tagging")]
public record AwsS3controlGetBucketTaggingOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--bucket")] string Bucket
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
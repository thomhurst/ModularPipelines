using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "create-access-point")]
public record AwsS3controlCreateAccessPointOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--name")] string Name,
[property: CliOption("--bucket")] string Bucket
) : AwsOptions
{
    [CliOption("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CliOption("--public-access-block-configuration")]
    public string? PublicAccessBlockConfiguration { get; set; }

    [CliOption("--bucket-account-id")]
    public string? BucketAccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
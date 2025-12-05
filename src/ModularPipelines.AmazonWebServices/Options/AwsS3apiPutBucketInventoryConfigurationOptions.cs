using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "put-bucket-inventory-configuration")]
public record AwsS3apiPutBucketInventoryConfigurationOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--id")] string Id,
[property: CliOption("--inventory-configuration")] string InventoryConfiguration
) : AwsOptions
{
    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
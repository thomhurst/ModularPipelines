using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "put-bucket-versioning")]
public record AwsS3controlPutBucketVersioningOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--versioning-configuration")] string VersioningConfiguration
) : AwsOptions
{
    [CliOption("--mfa")]
    public string? Mfa { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
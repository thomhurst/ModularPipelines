using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "delete-object")]
public record AwsS3apiDeleteObjectOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--key")] string Key
) : AwsOptions
{
    [CliOption("--mfa")]
    public string? Mfa { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--request-payer")]
    public string? RequestPayer { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
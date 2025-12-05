using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "restore-object")]
public record AwsS3apiRestoreObjectOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--key")] string Key
) : AwsOptions
{
    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--restore-request")]
    public string? RestoreRequest { get; set; }

    [CliOption("--request-payer")]
    public string? RequestPayer { get; set; }

    [CliOption("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
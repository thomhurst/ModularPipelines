using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "create-trust-store")]
public record AwsElbv2CreateTrustStoreOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--ca-certificates-bundle-s3-bucket")] string CaCertificatesBundleS3Bucket,
[property: CliOption("--ca-certificates-bundle-s3-key")] string CaCertificatesBundleS3Key
) : AwsOptions
{
    [CliOption("--ca-certificates-bundle-s3-object-version")]
    public string? CaCertificatesBundleS3ObjectVersion { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
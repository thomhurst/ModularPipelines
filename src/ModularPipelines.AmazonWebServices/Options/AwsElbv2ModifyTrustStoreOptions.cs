using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "modify-trust-store")]
public record AwsElbv2ModifyTrustStoreOptions(
[property: CliOption("--trust-store-arn")] string TrustStoreArn,
[property: CliOption("--ca-certificates-bundle-s3-bucket")] string CaCertificatesBundleS3Bucket,
[property: CliOption("--ca-certificates-bundle-s3-key")] string CaCertificatesBundleS3Key
) : AwsOptions
{
    [CliOption("--ca-certificates-bundle-s3-object-version")]
    public string? CaCertificatesBundleS3ObjectVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "modify-trust-store")]
public record AwsElbv2ModifyTrustStoreOptions(
[property: CommandSwitch("--trust-store-arn")] string TrustStoreArn,
[property: CommandSwitch("--ca-certificates-bundle-s3-bucket")] string CaCertificatesBundleS3Bucket,
[property: CommandSwitch("--ca-certificates-bundle-s3-key")] string CaCertificatesBundleS3Key
) : AwsOptions
{
    [CommandSwitch("--ca-certificates-bundle-s3-object-version")]
    public string? CaCertificatesBundleS3ObjectVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
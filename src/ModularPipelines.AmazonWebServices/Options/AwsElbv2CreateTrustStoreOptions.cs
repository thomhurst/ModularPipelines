using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "create-trust-store")]
public record AwsElbv2CreateTrustStoreOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--ca-certificates-bundle-s3-bucket")] string CaCertificatesBundleS3Bucket,
[property: CommandSwitch("--ca-certificates-bundle-s3-key")] string CaCertificatesBundleS3Key
) : AwsOptions
{
    [CommandSwitch("--ca-certificates-bundle-s3-object-version")]
    public string? CaCertificatesBundleS3ObjectVersion { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
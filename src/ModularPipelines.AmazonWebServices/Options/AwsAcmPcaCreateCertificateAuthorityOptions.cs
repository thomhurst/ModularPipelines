using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm-pca", "create-certificate-authority")]
public record AwsAcmPcaCreateCertificateAuthorityOptions(
[property: CliOption("--certificate-authority-configuration")] string CertificateAuthorityConfiguration,
[property: CliOption("--certificate-authority-type")] string CertificateAuthorityType
) : AwsOptions
{
    [CliOption("--revocation-configuration")]
    public string? RevocationConfiguration { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--key-storage-security-standard")]
    public string? KeyStorageSecurityStandard { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--usage-mode")]
    public string? UsageMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
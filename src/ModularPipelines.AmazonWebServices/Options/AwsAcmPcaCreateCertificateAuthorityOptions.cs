using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm-pca", "create-certificate-authority")]
public record AwsAcmPcaCreateCertificateAuthorityOptions(
[property: CommandSwitch("--certificate-authority-configuration")] string CertificateAuthorityConfiguration,
[property: CommandSwitch("--certificate-authority-type")] string CertificateAuthorityType
) : AwsOptions
{
    [CommandSwitch("--revocation-configuration")]
    public string? RevocationConfiguration { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--key-storage-security-standard")]
    public string? KeyStorageSecurityStandard { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--usage-mode")]
    public string? UsageMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
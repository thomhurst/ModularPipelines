using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm-pca", "update-certificate-authority")]
public record AwsAcmPcaUpdateCertificateAuthorityOptions(
[property: CliOption("--certificate-authority-arn")] string CertificateAuthorityArn
) : AwsOptions
{
    [CliOption("--revocation-configuration")]
    public string? RevocationConfiguration { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
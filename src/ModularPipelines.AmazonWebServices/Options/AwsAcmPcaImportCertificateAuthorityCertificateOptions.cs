using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm-pca", "import-certificate-authority-certificate")]
public record AwsAcmPcaImportCertificateAuthorityCertificateOptions(
[property: CliOption("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CliOption("--certificate")] string Certificate
) : AwsOptions
{
    [CliOption("--certificate-chain")]
    public string? CertificateChain { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm-pca", "wait", "certificate-authority-csr-created")]
public record AwsAcmPcaWaitCertificateAuthorityCsrCreatedOptions(
[property: CliOption("--certificate-authority-arn")] string CertificateAuthorityArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
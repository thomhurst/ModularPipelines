using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm-pca", "get-certificate-authority-certificate")]
public record AwsAcmPcaGetCertificateAuthorityCertificateOptions(
[property: CliOption("--certificate-authority-arn")] string CertificateAuthorityArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm-pca", "describe-certificate-authority")]
public record AwsAcmPcaDescribeCertificateAuthorityOptions(
[property: CliOption("--certificate-authority-arn")] string CertificateAuthorityArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
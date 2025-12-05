using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm-pca", "wait", "certificate-issued")]
public record AwsAcmPcaWaitCertificateIssuedOptions(
[property: CliOption("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CliOption("--certificate-arn")] string CertificateArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
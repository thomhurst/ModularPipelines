using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm-pca", "get-certificate-authority-csr")]
public record AwsAcmPcaGetCertificateAuthorityCsrOptions(
[property: CommandSwitch("--certificate-authority-arn")] string CertificateAuthorityArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
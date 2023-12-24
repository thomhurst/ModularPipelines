using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm-pca", "get-certificate")]
public record AwsAcmPcaGetCertificateOptions(
[property: CommandSwitch("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CommandSwitch("--certificate-arn")] string CertificateArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
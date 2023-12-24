using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm-pca", "wait", "certificate-issued")]
public record AwsAcmPcaWaitCertificateIssuedOptions(
[property: CommandSwitch("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CommandSwitch("--certificate-arn")] string CertificateArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
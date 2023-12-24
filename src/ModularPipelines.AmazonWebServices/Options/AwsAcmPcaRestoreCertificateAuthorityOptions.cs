using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm-pca", "restore-certificate-authority")]
public record AwsAcmPcaRestoreCertificateAuthorityOptions(
[property: CommandSwitch("--certificate-authority-arn")] string CertificateAuthorityArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm-pca", "import-certificate-authority-certificate")]
public record AwsAcmPcaImportCertificateAuthorityCertificateOptions(
[property: CommandSwitch("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CommandSwitch("--certificate")] string Certificate
) : AwsOptions
{
    [CommandSwitch("--certificate-chain")]
    public string? CertificateChain { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
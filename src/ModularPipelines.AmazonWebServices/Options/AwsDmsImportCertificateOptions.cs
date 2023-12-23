using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "import-certificate")]
public record AwsDmsImportCertificateOptions(
[property: CommandSwitch("--certificate-identifier")] string CertificateIdentifier
) : AwsOptions
{
    [CommandSwitch("--certificate-pem")]
    public string? CertificatePem { get; set; }

    [CommandSwitch("--certificate-wallet")]
    public string? CertificateWallet { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
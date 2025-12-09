using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "import-certificate")]
public record AwsDmsImportCertificateOptions(
[property: CliOption("--certificate-identifier")] string CertificateIdentifier
) : AwsOptions
{
    [CliOption("--certificate-pem")]
    public string? CertificatePem { get; set; }

    [CliOption("--certificate-wallet")]
    public string? CertificateWallet { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-certificate-from-csr")]
public record AwsIotCreateCertificateFromCsrOptions(
[property: CliOption("--certificate-signing-request")] string CertificateSigningRequest
) : AwsOptions
{
    [CliOption("--certificate-pem-outfile")]
    public string? CertificatePemOutfile { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-certificate-from-csr")]
public record AwsIotCreateCertificateFromCsrOptions(
[property: CommandSwitch("--certificate-signing-request")] string CertificateSigningRequest
) : AwsOptions
{
    [CommandSwitch("--certificate-pem-outfile")]
    public string? CertificatePemOutfile { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "register-certificate")]
public record AwsIotRegisterCertificateOptions(
[property: CliOption("--certificate-pem")] string CertificatePem
) : AwsOptions
{
    [CliOption("--ca-certificate-pem")]
    public string? CaCertificatePem { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
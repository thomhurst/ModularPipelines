using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "register-ca-certificate")]
public record AwsIotRegisterCaCertificateOptions(
[property: CliOption("--ca-certificate")] string CaCertificate
) : AwsOptions
{
    [CliOption("--verification-certificate")]
    public string? VerificationCertificate { get; set; }

    [CliOption("--registration-config")]
    public string? RegistrationConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--certificate-mode")]
    public string? CertificateMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
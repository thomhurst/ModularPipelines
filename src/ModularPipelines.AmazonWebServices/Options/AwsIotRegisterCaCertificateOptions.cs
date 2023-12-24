using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "register-ca-certificate")]
public record AwsIotRegisterCaCertificateOptions(
[property: CommandSwitch("--ca-certificate")] string CaCertificate
) : AwsOptions
{
    [CommandSwitch("--verification-certificate")]
    public string? VerificationCertificate { get; set; }

    [CommandSwitch("--registration-config")]
    public string? RegistrationConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--certificate-mode")]
    public string? CertificateMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
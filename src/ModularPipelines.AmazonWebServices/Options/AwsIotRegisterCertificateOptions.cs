using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "register-certificate")]
public record AwsIotRegisterCertificateOptions(
[property: CommandSwitch("--certificate-pem")] string CertificatePem
) : AwsOptions
{
    [CommandSwitch("--ca-certificate-pem")]
    public string? CaCertificatePem { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
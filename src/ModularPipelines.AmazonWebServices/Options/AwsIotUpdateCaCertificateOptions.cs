using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-ca-certificate")]
public record AwsIotUpdateCaCertificateOptions(
[property: CommandSwitch("--certificate-id")] string CertificateId
) : AwsOptions
{
    [CommandSwitch("--new-status")]
    public string? NewStatus { get; set; }

    [CommandSwitch("--new-auto-registration-status")]
    public string? NewAutoRegistrationStatus { get; set; }

    [CommandSwitch("--registration-config")]
    public string? RegistrationConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-ca-certificate")]
public record AwsIotUpdateCaCertificateOptions(
[property: CliOption("--certificate-id")] string CertificateId
) : AwsOptions
{
    [CliOption("--new-status")]
    public string? NewStatus { get; set; }

    [CliOption("--new-auto-registration-status")]
    public string? NewAutoRegistrationStatus { get; set; }

    [CliOption("--registration-config")]
    public string? RegistrationConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
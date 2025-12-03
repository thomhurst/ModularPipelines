using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-certificates")]
public record AwsLightsailGetCertificatesOptions : AwsOptions
{
    [CliOption("--certificate-statuses")]
    public string[]? CertificateStatuses { get; set; }

    [CliOption("--certificate-name")]
    public string? CertificateName { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
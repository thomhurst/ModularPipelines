using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "attach-certificate-to-distribution")]
public record AwsLightsailAttachCertificateToDistributionOptions(
[property: CliOption("--distribution-name")] string DistributionName,
[property: CliOption("--certificate-name")] string CertificateName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
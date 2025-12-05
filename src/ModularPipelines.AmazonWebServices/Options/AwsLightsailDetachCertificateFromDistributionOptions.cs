using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "detach-certificate-from-distribution")]
public record AwsLightsailDetachCertificateFromDistributionOptions(
[property: CliOption("--distribution-name")] string DistributionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
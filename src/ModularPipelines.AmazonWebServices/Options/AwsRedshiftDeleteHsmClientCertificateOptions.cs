using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "delete-hsm-client-certificate")]
public record AwsRedshiftDeleteHsmClientCertificateOptions(
[property: CliOption("--hsm-client-certificate-identifier")] string HsmClientCertificateIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-hsm-client-certificate")]
public record AwsRedshiftCreateHsmClientCertificateOptions(
[property: CliOption("--hsm-client-certificate-identifier")] string HsmClientCertificateIdentifier
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
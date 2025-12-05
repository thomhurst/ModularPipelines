using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm", "export-certificate")]
public record AwsAcmExportCertificateOptions(
[property: CliOption("--certificate-arn")] string CertificateArn,
[property: CliOption("--passphrase")] string Passphrase
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
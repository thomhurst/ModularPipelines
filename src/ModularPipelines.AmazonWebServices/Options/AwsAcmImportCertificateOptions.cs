using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm", "import-certificate")]
public record AwsAcmImportCertificateOptions(
[property: CliOption("--private-key")] string PrivateKey
) : AwsOptions
{
    [CliOption("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CliOption("--certificate")]
    public string? Certificate { get; set; }

    [CliOption("--certificate-chain")]
    public string? CertificateChain { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
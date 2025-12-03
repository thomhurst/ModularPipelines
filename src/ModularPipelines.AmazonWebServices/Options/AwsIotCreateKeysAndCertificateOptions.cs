using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-keys-and-certificate")]
public record AwsIotCreateKeysAndCertificateOptions : AwsOptions
{
    [CliOption("--certificate-pem-outfile")]
    public string? CertificatePemOutfile { get; set; }

    [CliOption("--public-key-outfile")]
    public string? PublicKeyOutfile { get; set; }

    [CliOption("--private-key-outfile")]
    public string? PrivateKeyOutfile { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
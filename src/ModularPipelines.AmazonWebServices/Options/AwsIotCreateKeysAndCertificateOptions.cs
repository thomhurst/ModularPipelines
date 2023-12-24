using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-keys-and-certificate")]
public record AwsIotCreateKeysAndCertificateOptions : AwsOptions
{
    [CommandSwitch("--certificate-pem-outfile")]
    public string? CertificatePemOutfile { get; set; }

    [CommandSwitch("--public-key-outfile")]
    public string? PublicKeyOutfile { get; set; }

    [CommandSwitch("--private-key-outfile")]
    public string? PrivateKeyOutfile { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
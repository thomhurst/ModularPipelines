using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography", "export-key")]
public record AwsPaymentCryptographyExportKeyOptions(
[property: CliOption("--export-key-identifier")] string ExportKeyIdentifier,
[property: CliOption("--key-material")] string KeyMaterial
) : AwsOptions
{
    [CliOption("--export-attributes")]
    public string? ExportAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
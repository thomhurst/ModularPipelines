using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography", "import-key")]
public record AwsPaymentCryptographyImportKeyOptions(
[property: CliOption("--key-material")] string KeyMaterial
) : AwsOptions
{
    [CliOption("--key-check-value-algorithm")]
    public string? KeyCheckValueAlgorithm { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
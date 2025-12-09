using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography", "get-parameters-for-export")]
public record AwsPaymentCryptographyGetParametersForExportOptions(
[property: CliOption("--key-material-type")] string KeyMaterialType,
[property: CliOption("--signing-key-algorithm")] string SigningKeyAlgorithm
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
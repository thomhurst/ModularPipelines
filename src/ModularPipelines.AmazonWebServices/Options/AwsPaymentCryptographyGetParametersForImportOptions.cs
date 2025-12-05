using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography", "get-parameters-for-import")]
public record AwsPaymentCryptographyGetParametersForImportOptions(
[property: CliOption("--key-material-type")] string KeyMaterialType,
[property: CliOption("--wrapping-key-algorithm")] string WrappingKeyAlgorithm
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
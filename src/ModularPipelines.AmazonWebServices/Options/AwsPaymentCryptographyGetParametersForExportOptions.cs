using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography", "get-parameters-for-export")]
public record AwsPaymentCryptographyGetParametersForExportOptions(
[property: CommandSwitch("--key-material-type")] string KeyMaterialType,
[property: CommandSwitch("--signing-key-algorithm")] string SigningKeyAlgorithm
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography", "get-parameters-for-import")]
public record AwsPaymentCryptographyGetParametersForImportOptions(
[property: CommandSwitch("--key-material-type")] string KeyMaterialType,
[property: CommandSwitch("--wrapping-key-algorithm")] string WrappingKeyAlgorithm
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
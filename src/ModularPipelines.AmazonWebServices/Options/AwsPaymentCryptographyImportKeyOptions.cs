using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography", "import-key")]
public record AwsPaymentCryptographyImportKeyOptions(
[property: CommandSwitch("--key-material")] string KeyMaterial
) : AwsOptions
{
    [CommandSwitch("--key-check-value-algorithm")]
    public string? KeyCheckValueAlgorithm { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
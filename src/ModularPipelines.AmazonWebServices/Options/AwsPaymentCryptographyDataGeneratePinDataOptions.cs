using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography-data", "generate-pin-data")]
public record AwsPaymentCryptographyDataGeneratePinDataOptions(
[property: CommandSwitch("--encryption-key-identifier")] string EncryptionKeyIdentifier,
[property: CommandSwitch("--generation-attributes")] string GenerationAttributes,
[property: CommandSwitch("--generation-key-identifier")] string GenerationKeyIdentifier,
[property: CommandSwitch("--pin-block-format")] string PinBlockFormat,
[property: CommandSwitch("--primary-account-number")] string PrimaryAccountNumber
) : AwsOptions
{
    [CommandSwitch("--pin-data-length")]
    public int? PinDataLength { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography-data", "decrypt-data")]
public record AwsPaymentCryptographyDataDecryptDataOptions(
[property: CommandSwitch("--cipher-text")] string CipherText,
[property: CommandSwitch("--decryption-attributes")] string DecryptionAttributes,
[property: CommandSwitch("--key-identifier")] string KeyIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
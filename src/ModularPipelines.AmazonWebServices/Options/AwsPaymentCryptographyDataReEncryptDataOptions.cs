using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography-data", "re-encrypt-data")]
public record AwsPaymentCryptographyDataReEncryptDataOptions(
[property: CommandSwitch("--cipher-text")] string CipherText,
[property: CommandSwitch("--incoming-encryption-attributes")] string IncomingEncryptionAttributes,
[property: CommandSwitch("--incoming-key-identifier")] string IncomingKeyIdentifier,
[property: CommandSwitch("--outgoing-encryption-attributes")] string OutgoingEncryptionAttributes,
[property: CommandSwitch("--outgoing-key-identifier")] string OutgoingKeyIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
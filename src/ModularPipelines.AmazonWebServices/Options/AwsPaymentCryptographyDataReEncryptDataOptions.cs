using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography-data", "re-encrypt-data")]
public record AwsPaymentCryptographyDataReEncryptDataOptions(
[property: CliOption("--cipher-text")] string CipherText,
[property: CliOption("--incoming-encryption-attributes")] string IncomingEncryptionAttributes,
[property: CliOption("--incoming-key-identifier")] string IncomingKeyIdentifier,
[property: CliOption("--outgoing-encryption-attributes")] string OutgoingEncryptionAttributes,
[property: CliOption("--outgoing-key-identifier")] string OutgoingKeyIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
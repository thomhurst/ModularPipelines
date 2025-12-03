using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography-data", "decrypt-data")]
public record AwsPaymentCryptographyDataDecryptDataOptions(
[property: CliOption("--cipher-text")] string CipherText,
[property: CliOption("--decryption-attributes")] string DecryptionAttributes,
[property: CliOption("--key-identifier")] string KeyIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
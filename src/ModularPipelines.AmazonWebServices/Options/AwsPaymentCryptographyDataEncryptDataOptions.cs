using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography-data", "encrypt-data")]
public record AwsPaymentCryptographyDataEncryptDataOptions(
[property: CliOption("--encryption-attributes")] string EncryptionAttributes,
[property: CliOption("--key-identifier")] string KeyIdentifier,
[property: CliOption("--plain-text")] string PlainText
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
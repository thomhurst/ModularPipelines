using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography-data", "generate-pin-data")]
public record AwsPaymentCryptographyDataGeneratePinDataOptions(
[property: CliOption("--encryption-key-identifier")] string EncryptionKeyIdentifier,
[property: CliOption("--generation-attributes")] string GenerationAttributes,
[property: CliOption("--generation-key-identifier")] string GenerationKeyIdentifier,
[property: CliOption("--pin-block-format")] string PinBlockFormat,
[property: CliOption("--primary-account-number")] string PrimaryAccountNumber
) : AwsOptions
{
    [CliOption("--pin-data-length")]
    public int? PinDataLength { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
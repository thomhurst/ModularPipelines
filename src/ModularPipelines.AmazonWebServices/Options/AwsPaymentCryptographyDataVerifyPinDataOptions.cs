using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography-data", "verify-pin-data")]
public record AwsPaymentCryptographyDataVerifyPinDataOptions(
[property: CliOption("--encrypted-pin-block")] string EncryptedPinBlock,
[property: CliOption("--encryption-key-identifier")] string EncryptionKeyIdentifier,
[property: CliOption("--pin-block-format")] string PinBlockFormat,
[property: CliOption("--primary-account-number")] string PrimaryAccountNumber,
[property: CliOption("--verification-attributes")] string VerificationAttributes,
[property: CliOption("--verification-key-identifier")] string VerificationKeyIdentifier
) : AwsOptions
{
    [CliOption("--dukpt-attributes")]
    public string? DukptAttributes { get; set; }

    [CliOption("--pin-data-length")]
    public int? PinDataLength { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
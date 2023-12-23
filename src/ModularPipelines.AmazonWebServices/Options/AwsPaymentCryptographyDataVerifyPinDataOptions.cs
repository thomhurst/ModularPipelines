using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography-data", "verify-pin-data")]
public record AwsPaymentCryptographyDataVerifyPinDataOptions(
[property: CommandSwitch("--encrypted-pin-block")] string EncryptedPinBlock,
[property: CommandSwitch("--encryption-key-identifier")] string EncryptionKeyIdentifier,
[property: CommandSwitch("--pin-block-format")] string PinBlockFormat,
[property: CommandSwitch("--primary-account-number")] string PrimaryAccountNumber,
[property: CommandSwitch("--verification-attributes")] string VerificationAttributes,
[property: CommandSwitch("--verification-key-identifier")] string VerificationKeyIdentifier
) : AwsOptions
{
    [CommandSwitch("--dukpt-attributes")]
    public string? DukptAttributes { get; set; }

    [CommandSwitch("--pin-data-length")]
    public int? PinDataLength { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
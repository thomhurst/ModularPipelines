using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography-data", "translate-pin-data")]
public record AwsPaymentCryptographyDataTranslatePinDataOptions(
[property: CliOption("--encrypted-pin-block")] string EncryptedPinBlock,
[property: CliOption("--incoming-key-identifier")] string IncomingKeyIdentifier,
[property: CliOption("--incoming-translation-attributes")] string IncomingTranslationAttributes,
[property: CliOption("--outgoing-key-identifier")] string OutgoingKeyIdentifier,
[property: CliOption("--outgoing-translation-attributes")] string OutgoingTranslationAttributes
) : AwsOptions
{
    [CliOption("--incoming-dukpt-attributes")]
    public string? IncomingDukptAttributes { get; set; }

    [CliOption("--outgoing-dukpt-attributes")]
    public string? OutgoingDukptAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography-data", "translate-pin-data")]
public record AwsPaymentCryptographyDataTranslatePinDataOptions(
[property: CommandSwitch("--encrypted-pin-block")] string EncryptedPinBlock,
[property: CommandSwitch("--incoming-key-identifier")] string IncomingKeyIdentifier,
[property: CommandSwitch("--incoming-translation-attributes")] string IncomingTranslationAttributes,
[property: CommandSwitch("--outgoing-key-identifier")] string OutgoingKeyIdentifier,
[property: CommandSwitch("--outgoing-translation-attributes")] string OutgoingTranslationAttributes
) : AwsOptions
{
    [CommandSwitch("--incoming-dukpt-attributes")]
    public string? IncomingDukptAttributes { get; set; }

    [CommandSwitch("--outgoing-dukpt-attributes")]
    public string? OutgoingDukptAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
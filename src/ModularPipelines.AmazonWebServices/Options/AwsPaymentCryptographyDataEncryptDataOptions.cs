using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography-data", "encrypt-data")]
public record AwsPaymentCryptographyDataEncryptDataOptions(
[property: CommandSwitch("--encryption-attributes")] string EncryptionAttributes,
[property: CommandSwitch("--key-identifier")] string KeyIdentifier,
[property: CommandSwitch("--plain-text")] string PlainText
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
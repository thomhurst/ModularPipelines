using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography-data", "verify-mac")]
public record AwsPaymentCryptographyDataVerifyMacOptions(
[property: CommandSwitch("--key-identifier")] string KeyIdentifier,
[property: CommandSwitch("--mac")] string Mac,
[property: CommandSwitch("--message-data")] string MessageData,
[property: CommandSwitch("--verification-attributes")] string VerificationAttributes
) : AwsOptions
{
    [CommandSwitch("--mac-length")]
    public int? MacLength { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
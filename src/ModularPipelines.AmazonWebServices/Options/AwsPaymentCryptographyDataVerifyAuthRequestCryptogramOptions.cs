using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography-data", "verify-auth-request-cryptogram")]
public record AwsPaymentCryptographyDataVerifyAuthRequestCryptogramOptions(
[property: CommandSwitch("--auth-request-cryptogram")] string AuthRequestCryptogram,
[property: CommandSwitch("--key-identifier")] string KeyIdentifier,
[property: CommandSwitch("--major-key-derivation-mode")] string MajorKeyDerivationMode,
[property: CommandSwitch("--session-key-derivation-attributes")] string SessionKeyDerivationAttributes,
[property: CommandSwitch("--transaction-data")] string TransactionData
) : AwsOptions
{
    [CommandSwitch("--auth-response-attributes")]
    public string? AuthResponseAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
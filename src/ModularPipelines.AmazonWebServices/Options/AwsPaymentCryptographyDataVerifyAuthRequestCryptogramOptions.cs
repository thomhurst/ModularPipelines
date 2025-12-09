using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography-data", "verify-auth-request-cryptogram")]
public record AwsPaymentCryptographyDataVerifyAuthRequestCryptogramOptions(
[property: CliOption("--auth-request-cryptogram")] string AuthRequestCryptogram,
[property: CliOption("--key-identifier")] string KeyIdentifier,
[property: CliOption("--major-key-derivation-mode")] string MajorKeyDerivationMode,
[property: CliOption("--session-key-derivation-attributes")] string SessionKeyDerivationAttributes,
[property: CliOption("--transaction-data")] string TransactionData
) : AwsOptions
{
    [CliOption("--auth-response-attributes")]
    public string? AuthResponseAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography-data", "verify-card-validation-data")]
public record AwsPaymentCryptographyDataVerifyCardValidationDataOptions(
[property: CommandSwitch("--key-identifier")] string KeyIdentifier,
[property: CommandSwitch("--primary-account-number")] string PrimaryAccountNumber,
[property: CommandSwitch("--validation-data")] string ValidationData,
[property: CommandSwitch("--verification-attributes")] string VerificationAttributes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
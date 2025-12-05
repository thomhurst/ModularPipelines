using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography-data", "verify-card-validation-data")]
public record AwsPaymentCryptographyDataVerifyCardValidationDataOptions(
[property: CliOption("--key-identifier")] string KeyIdentifier,
[property: CliOption("--primary-account-number")] string PrimaryAccountNumber,
[property: CliOption("--validation-data")] string ValidationData,
[property: CliOption("--verification-attributes")] string VerificationAttributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
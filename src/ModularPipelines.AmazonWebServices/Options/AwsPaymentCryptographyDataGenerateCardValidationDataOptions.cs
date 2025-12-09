using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography-data", "generate-card-validation-data")]
public record AwsPaymentCryptographyDataGenerateCardValidationDataOptions(
[property: CliOption("--generation-attributes")] string GenerationAttributes,
[property: CliOption("--key-identifier")] string KeyIdentifier,
[property: CliOption("--primary-account-number")] string PrimaryAccountNumber
) : AwsOptions
{
    [CliOption("--validation-data-length")]
    public int? ValidationDataLength { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography-data", "generate-card-validation-data")]
public record AwsPaymentCryptographyDataGenerateCardValidationDataOptions(
[property: CommandSwitch("--generation-attributes")] string GenerationAttributes,
[property: CommandSwitch("--key-identifier")] string KeyIdentifier,
[property: CommandSwitch("--primary-account-number")] string PrimaryAccountNumber
) : AwsOptions
{
    [CommandSwitch("--validation-data-length")]
    public int? ValidationDataLength { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
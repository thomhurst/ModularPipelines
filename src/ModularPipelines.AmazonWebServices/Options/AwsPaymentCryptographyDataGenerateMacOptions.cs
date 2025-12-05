using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography-data", "generate-mac")]
public record AwsPaymentCryptographyDataGenerateMacOptions(
[property: CliOption("--generation-attributes")] string GenerationAttributes,
[property: CliOption("--key-identifier")] string KeyIdentifier,
[property: CliOption("--message-data")] string MessageData
) : AwsOptions
{
    [CliOption("--mac-length")]
    public int? MacLength { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
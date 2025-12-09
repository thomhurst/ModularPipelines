using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography", "create-key")]
public record AwsPaymentCryptographyCreateKeyOptions(
[property: CliOption("--key-attributes")] string KeyAttributes
) : AwsOptions
{
    [CliOption("--key-check-value-algorithm")]
    public string? KeyCheckValueAlgorithm { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
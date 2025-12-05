using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography", "delete-key")]
public record AwsPaymentCryptographyDeleteKeyOptions(
[property: CliOption("--key-identifier")] string KeyIdentifier
) : AwsOptions
{
    [CliOption("--delete-key-in-days")]
    public int? DeleteKeyInDays { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
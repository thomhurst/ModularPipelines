using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography-data", "verify-mac")]
public record AwsPaymentCryptographyDataVerifyMacOptions(
[property: CliOption("--key-identifier")] string KeyIdentifier,
[property: CliOption("--mac")] string Mac,
[property: CliOption("--message-data")] string MessageData,
[property: CliOption("--verification-attributes")] string VerificationAttributes
) : AwsOptions
{
    [CliOption("--mac-length")]
    public int? MacLength { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
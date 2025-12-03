using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("payment-cryptography", "create-alias")]
public record AwsPaymentCryptographyCreateAliasOptions(
[property: CliOption("--alias-name")] string AliasName
) : AwsOptions
{
    [CliOption("--key-arn")]
    public string? KeyArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
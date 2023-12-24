using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography", "create-alias")]
public record AwsPaymentCryptographyCreateAliasOptions(
[property: CommandSwitch("--alias-name")] string AliasName
) : AwsOptions
{
    [CommandSwitch("--key-arn")]
    public string? KeyArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
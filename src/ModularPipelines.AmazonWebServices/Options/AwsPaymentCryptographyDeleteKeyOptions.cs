using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography", "delete-key")]
public record AwsPaymentCryptographyDeleteKeyOptions(
[property: CommandSwitch("--key-identifier")] string KeyIdentifier
) : AwsOptions
{
    [CommandSwitch("--delete-key-in-days")]
    public int? DeleteKeyInDays { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
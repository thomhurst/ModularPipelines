using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography-data", "generate-mac")]
public record AwsPaymentCryptographyDataGenerateMacOptions(
[property: CommandSwitch("--generation-attributes")] string GenerationAttributes,
[property: CommandSwitch("--key-identifier")] string KeyIdentifier,
[property: CommandSwitch("--message-data")] string MessageData
) : AwsOptions
{
    [CommandSwitch("--mac-length")]
    public int? MacLength { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
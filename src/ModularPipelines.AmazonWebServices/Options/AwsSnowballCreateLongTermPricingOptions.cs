using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("snowball", "create-long-term-pricing")]
public record AwsSnowballCreateLongTermPricingOptions(
[property: CommandSwitch("--long-term-pricing-type")] string LongTermPricingType,
[property: CommandSwitch("--snowball-type")] string SnowballType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
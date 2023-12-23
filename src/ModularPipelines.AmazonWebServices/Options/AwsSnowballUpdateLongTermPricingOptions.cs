using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("snowball", "update-long-term-pricing")]
public record AwsSnowballUpdateLongTermPricingOptions(
[property: CommandSwitch("--long-term-pricing-id")] string LongTermPricingId
) : AwsOptions
{
    [CommandSwitch("--replacement-job")]
    public string? ReplacementJob { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
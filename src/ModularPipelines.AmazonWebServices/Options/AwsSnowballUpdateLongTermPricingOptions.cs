using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snowball", "update-long-term-pricing")]
public record AwsSnowballUpdateLongTermPricingOptions(
[property: CliOption("--long-term-pricing-id")] string LongTermPricingId
) : AwsOptions
{
    [CliOption("--replacement-job")]
    public string? ReplacementJob { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snowball", "create-long-term-pricing")]
public record AwsSnowballCreateLongTermPricingOptions(
[property: CliOption("--long-term-pricing-type")] string LongTermPricingType,
[property: CliOption("--snowball-type")] string SnowballType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
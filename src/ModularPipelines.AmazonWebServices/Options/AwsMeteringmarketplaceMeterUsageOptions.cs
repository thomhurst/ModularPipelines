using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("meteringmarketplace", "meter-usage")]
public record AwsMeteringmarketplaceMeterUsageOptions(
[property: CliOption("--product-code")] string ProductCode,
[property: CliOption("--timestamp")] long Timestamp,
[property: CliOption("--usage-dimension")] string UsageDimension
) : AwsOptions
{
    [CliOption("--usage-quantity")]
    public int? UsageQuantity { get; set; }

    [CliOption("--usage-allocations")]
    public string[]? UsageAllocations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("meteringmarketplace", "meter-usage")]
public record AwsMeteringmarketplaceMeterUsageOptions(
[property: CommandSwitch("--product-code")] string ProductCode,
[property: CommandSwitch("--timestamp")] long Timestamp,
[property: CommandSwitch("--usage-dimension")] string UsageDimension
) : AwsOptions
{
    [CommandSwitch("--usage-quantity")]
    public int? UsageQuantity { get; set; }

    [CommandSwitch("--usage-allocations")]
    public string[]? UsageAllocations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
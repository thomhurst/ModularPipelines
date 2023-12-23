using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("meteringmarketplace", "batch-meter-usage")]
public record AwsMeteringmarketplaceBatchMeterUsageOptions(
[property: CommandSwitch("--usage-records")] string[] UsageRecords,
[property: CommandSwitch("--product-code")] string ProductCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
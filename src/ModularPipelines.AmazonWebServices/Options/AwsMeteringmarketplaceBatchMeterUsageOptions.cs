using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("meteringmarketplace", "batch-meter-usage")]
public record AwsMeteringmarketplaceBatchMeterUsageOptions(
[property: CliOption("--usage-records")] string[] UsageRecords,
[property: CliOption("--product-code")] string ProductCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
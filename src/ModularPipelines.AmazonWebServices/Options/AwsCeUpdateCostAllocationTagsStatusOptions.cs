using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "update-cost-allocation-tags-status")]
public record AwsCeUpdateCostAllocationTagsStatusOptions(
[property: CliOption("--cost-allocation-tags-status")] string[] CostAllocationTagsStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
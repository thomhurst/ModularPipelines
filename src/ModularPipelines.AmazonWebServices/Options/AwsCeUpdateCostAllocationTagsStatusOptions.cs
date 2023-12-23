using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "update-cost-allocation-tags-status")]
public record AwsCeUpdateCostAllocationTagsStatusOptions(
[property: CommandSwitch("--cost-allocation-tags-status")] string[] CostAllocationTagsStatus
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
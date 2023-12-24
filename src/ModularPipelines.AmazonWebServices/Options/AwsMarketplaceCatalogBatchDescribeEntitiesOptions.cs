using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("marketplace-catalog", "batch-describe-entities")]
public record AwsMarketplaceCatalogBatchDescribeEntitiesOptions(
[property: CommandSwitch("--entity-request-list")] string[] EntityRequestList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
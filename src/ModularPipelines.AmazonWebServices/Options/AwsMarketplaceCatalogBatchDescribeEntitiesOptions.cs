using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("marketplace-catalog", "batch-describe-entities")]
public record AwsMarketplaceCatalogBatchDescribeEntitiesOptions(
[property: CliOption("--entity-request-list")] string[] EntityRequestList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
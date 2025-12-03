using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("marketplace-catalog", "describe-entity")]
public record AwsMarketplaceCatalogDescribeEntityOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--entity-id")] string EntityId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
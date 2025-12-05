using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("marketplace-catalog", "list-entities")]
public record AwsMarketplaceCatalogListEntitiesOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--entity-type")] string EntityType
) : AwsOptions
{
    [CliOption("--filter-list")]
    public string[]? FilterList { get; set; }

    [CliOption("--sort")]
    public string? Sort { get; set; }

    [CliOption("--ownership-type")]
    public string? OwnershipType { get; set; }

    [CliOption("--entity-type-filters")]
    public string? EntityTypeFilters { get; set; }

    [CliOption("--entity-type-sort")]
    public string? EntityTypeSort { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
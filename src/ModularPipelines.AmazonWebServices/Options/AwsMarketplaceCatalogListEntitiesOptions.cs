using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("marketplace-catalog", "list-entities")]
public record AwsMarketplaceCatalogListEntitiesOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--entity-type")] string EntityType
) : AwsOptions
{
    [CommandSwitch("--filter-list")]
    public string[]? FilterList { get; set; }

    [CommandSwitch("--sort")]
    public string? Sort { get; set; }

    [CommandSwitch("--ownership-type")]
    public string? OwnershipType { get; set; }

    [CommandSwitch("--entity-type-filters")]
    public string? EntityTypeFilters { get; set; }

    [CommandSwitch("--entity-type-sort")]
    public string? EntityTypeSort { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
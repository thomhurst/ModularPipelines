using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "search-tables")]
public record AwsGlueSearchTablesOptions : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--search-text")]
    public string? SearchText { get; set; }

    [CliOption("--sort-criteria")]
    public string[]? SortCriteria { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--resource-share-type")]
    public string? ResourceShareType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
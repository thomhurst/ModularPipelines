using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "search-provisioned-products")]
public record AwsServicecatalogSearchProvisionedProductsOptions : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--access-level-filter")]
    public string? AccessLevelFilter { get; set; }

    [CliOption("--filters")]
    public IEnumerable<KeyValue>? Filters { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
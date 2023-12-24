using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "search-products-as-admin")]
public record AwsServicecatalogSearchProductsAsAdminOptions : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--portfolio-id")]
    public string? PortfolioId { get; set; }

    [CommandSwitch("--filters")]
    public IEnumerable<KeyValue>? Filters { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--product-source")]
    public string? ProductSource { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "associate-product-with-portfolio")]
public record AwsServicecatalogAssociateProductWithPortfolioOptions(
[property: CliOption("--product-id")] string ProductId,
[property: CliOption("--portfolio-id")] string PortfolioId
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--source-portfolio-id")]
    public string? SourcePortfolioId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
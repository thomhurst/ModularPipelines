using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "list-accepted-portfolio-shares")]
public record AwsServicecatalogListAcceptedPortfolioSharesOptions : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--portfolio-share-type")]
    public string? PortfolioShareType { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
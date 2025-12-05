using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "describe-portfolio-shares")]
public record AwsServicecatalogDescribePortfolioSharesOptions(
[property: CliOption("--portfolio-id")] string PortfolioId,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
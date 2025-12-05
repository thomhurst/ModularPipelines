using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "describe-portfolio-share-status")]
public record AwsServicecatalogDescribePortfolioShareStatusOptions(
[property: CliOption("--portfolio-share-token")] string PortfolioShareToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
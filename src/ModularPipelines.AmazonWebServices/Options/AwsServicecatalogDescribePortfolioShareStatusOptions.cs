using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "describe-portfolio-share-status")]
public record AwsServicecatalogDescribePortfolioShareStatusOptions(
[property: CommandSwitch("--portfolio-share-token")] string PortfolioShareToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "reject-portfolio-share")]
public record AwsServicecatalogRejectPortfolioShareOptions(
[property: CommandSwitch("--portfolio-id")] string PortfolioId
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--portfolio-share-type")]
    public string? PortfolioShareType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
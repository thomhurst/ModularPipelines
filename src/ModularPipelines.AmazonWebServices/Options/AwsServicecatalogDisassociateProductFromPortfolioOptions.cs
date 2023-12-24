using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "disassociate-product-from-portfolio")]
public record AwsServicecatalogDisassociateProductFromPortfolioOptions(
[property: CommandSwitch("--product-id")] string ProductId,
[property: CommandSwitch("--portfolio-id")] string PortfolioId
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "delete-portfolio-share")]
public record AwsServicecatalogDeletePortfolioShareOptions(
[property: CommandSwitch("--portfolio-id")] string PortfolioId
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--organization-node")]
    public string? OrganizationNode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
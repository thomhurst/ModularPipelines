using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "describe-portfolio-shares")]
public record AwsServicecatalogDescribePortfolioSharesOptions(
[property: CommandSwitch("--portfolio-id")] string PortfolioId,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
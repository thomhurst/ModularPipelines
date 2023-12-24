using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "associate-principal-with-portfolio")]
public record AwsServicecatalogAssociatePrincipalWithPortfolioOptions(
[property: CommandSwitch("--portfolio-id")] string PortfolioId,
[property: CommandSwitch("--principal-arn")] string PrincipalArn,
[property: CommandSwitch("--principal-type")] string PrincipalType
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
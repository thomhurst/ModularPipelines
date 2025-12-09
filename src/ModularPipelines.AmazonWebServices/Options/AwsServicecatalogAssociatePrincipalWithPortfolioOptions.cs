using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "associate-principal-with-portfolio")]
public record AwsServicecatalogAssociatePrincipalWithPortfolioOptions(
[property: CliOption("--portfolio-id")] string PortfolioId,
[property: CliOption("--principal-arn")] string PrincipalArn,
[property: CliOption("--principal-type")] string PrincipalType
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
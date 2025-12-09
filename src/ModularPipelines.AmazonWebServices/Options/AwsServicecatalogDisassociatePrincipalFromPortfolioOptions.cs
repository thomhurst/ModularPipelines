using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "disassociate-principal-from-portfolio")]
public record AwsServicecatalogDisassociatePrincipalFromPortfolioOptions(
[property: CliOption("--portfolio-id")] string PortfolioId,
[property: CliOption("--principal-arn")] string PrincipalArn
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--principal-type")]
    public string? PrincipalType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
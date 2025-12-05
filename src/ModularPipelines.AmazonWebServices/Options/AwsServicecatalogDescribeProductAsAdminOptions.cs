using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "describe-product-as-admin")]
public record AwsServicecatalogDescribeProductAsAdminOptions : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--source-portfolio-id")]
    public string? SourcePortfolioId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
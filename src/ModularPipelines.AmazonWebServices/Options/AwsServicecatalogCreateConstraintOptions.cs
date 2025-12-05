using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "create-constraint")]
public record AwsServicecatalogCreateConstraintOptions(
[property: CliOption("--portfolio-id")] string PortfolioId,
[property: CliOption("--product-id")] string ProductId,
[property: CliOption("--parameters")] string Parameters,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
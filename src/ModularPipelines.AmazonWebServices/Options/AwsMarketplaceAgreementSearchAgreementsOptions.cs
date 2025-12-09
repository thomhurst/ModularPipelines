using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("marketplace-agreement", "search-agreements")]
public record AwsMarketplaceAgreementSearchAgreementsOptions : AwsOptions
{
    [CliOption("--catalog")]
    public string? Catalog { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--sort")]
    public string? Sort { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "get-similar-profiles")]
public record AwsCustomerProfilesGetSimilarProfilesOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--match-type")] string MatchType,
[property: CliOption("--search-key")] string SearchKey,
[property: CliOption("--search-value")] string SearchValue
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
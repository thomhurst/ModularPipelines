using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "get-similar-profiles")]
public record AwsCustomerProfilesGetSimilarProfilesOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--match-type")] string MatchType,
[property: CommandSwitch("--search-key")] string SearchKey,
[property: CommandSwitch("--search-value")] string SearchValue
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
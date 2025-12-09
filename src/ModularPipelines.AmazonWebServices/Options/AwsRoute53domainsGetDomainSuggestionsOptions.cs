using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "get-domain-suggestions")]
public record AwsRoute53domainsGetDomainSuggestionsOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--suggestion-count")] int SuggestionCount
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
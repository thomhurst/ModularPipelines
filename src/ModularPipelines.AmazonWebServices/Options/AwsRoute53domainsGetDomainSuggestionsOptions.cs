using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "get-domain-suggestions")]
public record AwsRoute53domainsGetDomainSuggestionsOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--suggestion-count")] int SuggestionCount
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
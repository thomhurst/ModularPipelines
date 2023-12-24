using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "get-query-suggestions")]
public record AwsKendraGetQuerySuggestionsOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--query-text")] string QueryText
) : AwsOptions
{
    [CommandSwitch("--max-suggestions-count")]
    public int? MaxSuggestionsCount { get; set; }

    [CommandSwitch("--suggestion-types")]
    public string[]? SuggestionTypes { get; set; }

    [CommandSwitch("--attribute-suggestions-config")]
    public string? AttributeSuggestionsConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
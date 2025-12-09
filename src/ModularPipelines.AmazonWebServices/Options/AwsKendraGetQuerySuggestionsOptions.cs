using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "get-query-suggestions")]
public record AwsKendraGetQuerySuggestionsOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--query-text")] string QueryText
) : AwsOptions
{
    [CliOption("--max-suggestions-count")]
    public int? MaxSuggestionsCount { get; set; }

    [CliOption("--suggestion-types")]
    public string[]? SuggestionTypes { get; set; }

    [CliOption("--attribute-suggestions-config")]
    public string? AttributeSuggestionsConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
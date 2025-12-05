using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "get-search-suggestions")]
public record AwsSagemakerGetSearchSuggestionsOptions(
[property: CliOption("--resource")] string Resource
) : AwsOptions
{
    [CliOption("--suggestion-query")]
    public string? SuggestionQuery { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
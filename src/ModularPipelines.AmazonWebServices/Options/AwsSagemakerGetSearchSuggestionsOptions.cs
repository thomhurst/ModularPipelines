using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "get-search-suggestions")]
public record AwsSagemakerGetSearchSuggestionsOptions(
[property: CommandSwitch("--resource")] string Resource
) : AwsOptions
{
    [CommandSwitch("--suggestion-query")]
    public string? SuggestionQuery { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
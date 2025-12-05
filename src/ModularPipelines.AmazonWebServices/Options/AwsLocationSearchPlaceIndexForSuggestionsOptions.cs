using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "search-place-index-for-suggestions")]
public record AwsLocationSearchPlaceIndexForSuggestionsOptions(
[property: CliOption("--index-name")] string IndexName,
[property: CliOption("--text")] string Text
) : AwsOptions
{
    [CliOption("--bias-position")]
    public string[]? BiasPosition { get; set; }

    [CliOption("--filter-b-box")]
    public string[]? FilterBBox { get; set; }

    [CliOption("--filter-categories")]
    public string[]? FilterCategories { get; set; }

    [CliOption("--filter-countries")]
    public string[]? FilterCountries { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
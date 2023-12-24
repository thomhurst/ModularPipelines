using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "search-place-index-for-text")]
public record AwsLocationSearchPlaceIndexForTextOptions(
[property: CommandSwitch("--index-name")] string IndexName,
[property: CommandSwitch("--text")] string Text
) : AwsOptions
{
    [CommandSwitch("--bias-position")]
    public string[]? BiasPosition { get; set; }

    [CommandSwitch("--filter-b-box")]
    public string[]? FilterBBox { get; set; }

    [CommandSwitch("--filter-categories")]
    public string[]? FilterCategories { get; set; }

    [CommandSwitch("--filter-countries")]
    public string[]? FilterCountries { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
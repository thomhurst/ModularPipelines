using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "search-place-index-for-position")]
public record AwsLocationSearchPlaceIndexForPositionOptions(
[property: CliOption("--index-name")] string IndexName,
[property: CliOption("--position")] string[] Position
) : AwsOptions
{
    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
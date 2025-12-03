using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "get-place")]
public record AwsLocationGetPlaceOptions(
[property: CliOption("--index-name")] string IndexName,
[property: CliOption("--place-id")] string PlaceId
) : AwsOptions
{
    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
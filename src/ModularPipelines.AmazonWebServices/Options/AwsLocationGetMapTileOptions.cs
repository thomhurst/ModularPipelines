using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "get-map-tile")]
public record AwsLocationGetMapTileOptions(
[property: CliOption("--map-name")] string MapName,
[property: CliOption("--x")] string X,
[property: CliOption("--y")] string Y,
[property: CliOption("--z")] string Z
) : AwsOptions
{
    [CliOption("--key")]
    public string? Key { get; set; }
}
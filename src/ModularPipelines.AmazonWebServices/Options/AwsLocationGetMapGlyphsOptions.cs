using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "get-map-glyphs")]
public record AwsLocationGetMapGlyphsOptions(
[property: CliOption("--font-stack")] string FontStack,
[property: CliOption("--font-unicode-range")] string FontUnicodeRange,
[property: CliOption("--map-name")] string MapName
) : AwsOptions
{
    [CliOption("--key")]
    public string? Key { get; set; }
}
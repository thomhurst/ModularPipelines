using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "get-map-glyphs")]
public record AwsLocationGetMapGlyphsOptions(
[property: CommandSwitch("--font-stack")] string FontStack,
[property: CommandSwitch("--font-unicode-range")] string FontUnicodeRange,
[property: CommandSwitch("--map-name")] string MapName
) : AwsOptions
{
    [CommandSwitch("--key")]
    public string? Key { get; set; }
}
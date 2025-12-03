using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "vision", "suggest-crop")]
public record GcloudMlVisionSuggestCropOptions(
[property: CliArgument] string ImagePath
) : GcloudOptions
{
    [CliOption("--aspect-ratios")]
    public string[]? AspectRatios { get; set; }
}
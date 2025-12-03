using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "vision", "detect-text")]
public record GcloudMlVisionDetectTextOptions(
[property: CliArgument] string ImagePath
) : GcloudOptions
{
    [CliOption("--language-hints")]
    public string[]? LanguageHints { get; set; }
}
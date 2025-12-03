using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "vision", "detect-document")]
public record GcloudMlVisionDetectDocumentOptions(
[property: CliArgument] string ImagePath
) : GcloudOptions
{
    [CliOption("--language-hints")]
    public string[]? LanguageHints { get; set; }
}
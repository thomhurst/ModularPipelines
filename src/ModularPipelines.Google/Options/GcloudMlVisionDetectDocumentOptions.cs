using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "vision", "detect-document")]
public record GcloudMlVisionDetectDocumentOptions(
[property: PositionalArgument] string ImagePath
) : GcloudOptions
{
    [CommandSwitch("--language-hints")]
    public string[]? LanguageHints { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "vision", "detect-text-pdf")]
public record GcloudMlVisionDetectTextPdfOptions(
[property: PositionalArgument] string InputFile,
[property: PositionalArgument] string OutputPath
) : GcloudOptions
{
    [CommandSwitch("--batch-size")]
    public string? BatchSize { get; set; }
}
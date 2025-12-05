using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "vision", "detect-text-tiff")]
public record GcloudMlVisionDetectTextTiffOptions(
[property: CliArgument] string InputFile,
[property: CliArgument] string OutputPath
) : GcloudOptions
{
    [CliOption("--batch-size")]
    public string? BatchSize { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "language", "classify-text")]
public record GcloudMlLanguageClassifyTextOptions(
[property: CliOption("--content")] string Content,
[property: CliOption("--content-file")] string ContentFile
) : GcloudOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }
}
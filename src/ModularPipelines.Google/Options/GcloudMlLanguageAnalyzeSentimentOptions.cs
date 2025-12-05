using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "language", "analyze-sentiment")]
public record GcloudMlLanguageAnalyzeSentimentOptions(
[property: CliOption("--content")] string Content,
[property: CliOption("--content-file")] string ContentFile
) : GcloudOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--encoding-type")]
    public string? EncodingType { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }
}
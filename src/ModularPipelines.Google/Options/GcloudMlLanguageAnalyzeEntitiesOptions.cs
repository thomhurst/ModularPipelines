using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "language", "analyze-entities")]
public record GcloudMlLanguageAnalyzeEntitiesOptions(
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--content-file")] string ContentFile
) : GcloudOptions
{
    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--encoding-type")]
    public string? EncodingType { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }
}
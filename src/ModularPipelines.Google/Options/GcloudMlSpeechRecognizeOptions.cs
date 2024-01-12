using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "speech", "recognize")]
public record GcloudMlSpeechRecognizeOptions(
[property: PositionalArgument] string Audio,
[property: CommandSwitch("--language-code")] string LanguageCode
) : GcloudOptions
{
    [BooleanCommandSwitch("--enable-automatic-punctuation")]
    public bool? EnableAutomaticPunctuation { get; set; }

    [CommandSwitch("--encoding")]
    public string? Encoding { get; set; }

    [BooleanCommandSwitch("--filter-profanity")]
    public bool? FilterProfanity { get; set; }

    [CommandSwitch("--hints")]
    public string[]? Hints { get; set; }

    [BooleanCommandSwitch("--include-word-time-offsets")]
    public bool? IncludeWordTimeOffsets { get; set; }

    [CommandSwitch("--max-alternatives")]
    public string? MaxAlternatives { get; set; }

    [CommandSwitch("--model")]
    public string? Model { get; set; }

    [CommandSwitch("--sample-rate")]
    public string? SampleRate { get; set; }

    [CommandSwitch("--audio-channel-count")]
    public string? AudioChannelCount { get; set; }

    [BooleanCommandSwitch("--separate-channel-recognition")]
    public bool? SeparateChannelRecognition { get; set; }
}
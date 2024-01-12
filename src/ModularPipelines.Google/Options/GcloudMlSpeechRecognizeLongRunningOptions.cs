using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "speech", "recognize-long-running")]
public record GcloudMlSpeechRecognizeLongRunningOptions(
[property: PositionalArgument] string Audio,
[property: CommandSwitch("--language-code")] string LanguageCode
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

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

    [CommandSwitch("--output-uri")]
    public string? OutputUri { get; set; }

    [CommandSwitch("--sample-rate")]
    public string? SampleRate { get; set; }

    [CommandSwitch("--audio-channel-count")]
    public string? AudioChannelCount { get; set; }

    [BooleanCommandSwitch("--separate-channel-recognition")]
    public bool? SeparateChannelRecognition { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "speech", "recognize-long-running")]
public record GcloudMlSpeechRecognizeLongRunningOptions(
[property: CliArgument] string Audio,
[property: CliOption("--language-code")] string LanguageCode
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--enable-automatic-punctuation")]
    public bool? EnableAutomaticPunctuation { get; set; }

    [CliOption("--encoding")]
    public string? Encoding { get; set; }

    [CliFlag("--filter-profanity")]
    public bool? FilterProfanity { get; set; }

    [CliOption("--hints")]
    public string[]? Hints { get; set; }

    [CliFlag("--include-word-time-offsets")]
    public bool? IncludeWordTimeOffsets { get; set; }

    [CliOption("--max-alternatives")]
    public string? MaxAlternatives { get; set; }

    [CliOption("--model")]
    public string? Model { get; set; }

    [CliOption("--output-uri")]
    public string? OutputUri { get; set; }

    [CliOption("--sample-rate")]
    public string? SampleRate { get; set; }

    [CliOption("--audio-channel-count")]
    public string? AudioChannelCount { get; set; }

    [CliFlag("--separate-channel-recognition")]
    public bool? SeparateChannelRecognition { get; set; }
}
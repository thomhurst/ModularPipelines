using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("polly", "synthesize-speech")]
public record AwsPollySynthesizeSpeechOptions(
[property: CliOption("--output-format")] string OutputFormat,
[property: CliOption("--text")] string Text,
[property: CliOption("--voice-id")] string VoiceId
) : AwsOptions
{
    [CliOption("--engine")]
    public string? Engine { get; set; }

    [CliOption("--language-code")]
    public string? LanguageCode { get; set; }

    [CliOption("--lexicon-names")]
    public string[]? LexiconNames { get; set; }

    [CliOption("--sample-rate")]
    public string? SampleRate { get; set; }

    [CliOption("--speech-mark-types")]
    public string[]? SpeechMarkTypes { get; set; }

    [CliOption("--text-type")]
    public string? TextType { get; set; }
}
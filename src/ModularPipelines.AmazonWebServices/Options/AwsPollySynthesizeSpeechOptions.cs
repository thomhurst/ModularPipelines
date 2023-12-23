using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("polly", "synthesize-speech")]
public record AwsPollySynthesizeSpeechOptions(
[property: CommandSwitch("--output-format")] string OutputFormat,
[property: CommandSwitch("--text")] string Text,
[property: CommandSwitch("--voice-id")] string VoiceId
) : AwsOptions
{
    [CommandSwitch("--engine")]
    public string? Engine { get; set; }

    [CommandSwitch("--language-code")]
    public string? LanguageCode { get; set; }

    [CommandSwitch("--lexicon-names")]
    public string[]? LexiconNames { get; set; }

    [CommandSwitch("--sample-rate")]
    public string? SampleRate { get; set; }

    [CommandSwitch("--speech-mark-types")]
    public string[]? SpeechMarkTypes { get; set; }

    [CommandSwitch("--text-type")]
    public string? TextType { get; set; }
}
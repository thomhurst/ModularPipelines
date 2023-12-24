using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("polly", "start-speech-synthesis-task")]
public record AwsPollyStartSpeechSynthesisTaskOptions(
[property: CommandSwitch("--output-format")] string OutputFormat,
[property: CommandSwitch("--output-s3-bucket-name")] string OutputS3BucketName,
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

    [CommandSwitch("--output-s3-key-prefix")]
    public string? OutputS3KeyPrefix { get; set; }

    [CommandSwitch("--sample-rate")]
    public string? SampleRate { get; set; }

    [CommandSwitch("--sns-topic-arn")]
    public string? SnsTopicArn { get; set; }

    [CommandSwitch("--speech-mark-types")]
    public string[]? SpeechMarkTypes { get; set; }

    [CommandSwitch("--text-type")]
    public string? TextType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
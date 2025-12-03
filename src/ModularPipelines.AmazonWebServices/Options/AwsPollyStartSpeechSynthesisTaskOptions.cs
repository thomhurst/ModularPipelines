using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("polly", "start-speech-synthesis-task")]
public record AwsPollyStartSpeechSynthesisTaskOptions(
[property: CliOption("--output-format")] string OutputFormat,
[property: CliOption("--output-s3-bucket-name")] string OutputS3BucketName,
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

    [CliOption("--output-s3-key-prefix")]
    public string? OutputS3KeyPrefix { get; set; }

    [CliOption("--sample-rate")]
    public string? SampleRate { get; set; }

    [CliOption("--sns-topic-arn")]
    public string? SnsTopicArn { get; set; }

    [CliOption("--speech-mark-types")]
    public string[]? SpeechMarkTypes { get; set; }

    [CliOption("--text-type")]
    public string? TextType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "start-transcription-job")]
public record AwsTranscribeStartTranscriptionJobOptions(
[property: CommandSwitch("--transcription-job-name")] string TranscriptionJobName
) : AwsOptions
{
    [CommandSwitch("--language-code")]
    public string? LanguageCode { get; set; }

    [CommandSwitch("--media-sample-rate-hertz")]
    public int? MediaSampleRateHertz { get; set; }

    [CommandSwitch("--media-format")]
    public string? MediaFormat { get; set; }

    [CommandSwitch("--media")]
    public string? Media { get; set; }

    [CommandSwitch("--output-bucket-name")]
    public string? OutputBucketName { get; set; }

    [CommandSwitch("--output-key")]
    public string? OutputKey { get; set; }

    [CommandSwitch("--output-encryption-kms-key-id")]
    public string? OutputEncryptionKmsKeyId { get; set; }

    [CommandSwitch("--kms-encryption-context")]
    public IEnumerable<KeyValue>? KmsEncryptionContext { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--model-settings")]
    public string? ModelSettings { get; set; }

    [CommandSwitch("--job-execution-settings")]
    public string? JobExecutionSettings { get; set; }

    [CommandSwitch("--content-redaction")]
    public string? ContentRedaction { get; set; }

    [CommandSwitch("--language-options")]
    public string[]? LanguageOptions { get; set; }

    [CommandSwitch("--subtitles")]
    public string? Subtitles { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--language-id-settings")]
    public IEnumerable<KeyValue>? LanguageIdSettings { get; set; }

    [CommandSwitch("--toxicity-detection")]
    public string[]? ToxicityDetection { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
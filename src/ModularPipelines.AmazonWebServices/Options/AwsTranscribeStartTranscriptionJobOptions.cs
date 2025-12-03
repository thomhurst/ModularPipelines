using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "start-transcription-job")]
public record AwsTranscribeStartTranscriptionJobOptions(
[property: CliOption("--transcription-job-name")] string TranscriptionJobName
) : AwsOptions
{
    [CliOption("--language-code")]
    public string? LanguageCode { get; set; }

    [CliOption("--media-sample-rate-hertz")]
    public int? MediaSampleRateHertz { get; set; }

    [CliOption("--media-format")]
    public string? MediaFormat { get; set; }

    [CliOption("--media")]
    public string? Media { get; set; }

    [CliOption("--output-bucket-name")]
    public string? OutputBucketName { get; set; }

    [CliOption("--output-key")]
    public string? OutputKey { get; set; }

    [CliOption("--output-encryption-kms-key-id")]
    public string? OutputEncryptionKmsKeyId { get; set; }

    [CliOption("--kms-encryption-context")]
    public IEnumerable<KeyValue>? KmsEncryptionContext { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--model-settings")]
    public string? ModelSettings { get; set; }

    [CliOption("--job-execution-settings")]
    public string? JobExecutionSettings { get; set; }

    [CliOption("--content-redaction")]
    public string? ContentRedaction { get; set; }

    [CliOption("--language-options")]
    public string[]? LanguageOptions { get; set; }

    [CliOption("--subtitles")]
    public string? Subtitles { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--language-id-settings")]
    public IEnumerable<KeyValue>? LanguageIdSettings { get; set; }

    [CliOption("--toxicity-detection")]
    public string[]? ToxicityDetection { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
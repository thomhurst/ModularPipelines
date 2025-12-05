using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "start-medical-transcription-job")]
public record AwsTranscribeStartMedicalTranscriptionJobOptions(
[property: CliOption("--medical-transcription-job-name")] string MedicalTranscriptionJobName,
[property: CliOption("--language-code")] string LanguageCode,
[property: CliOption("--output-bucket-name")] string OutputBucketName,
[property: CliOption("--specialty")] string Specialty,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--media-sample-rate-hertz")]
    public int? MediaSampleRateHertz { get; set; }

    [CliOption("--media-format")]
    public string? MediaFormat { get; set; }

    [CliOption("--media")]
    public string? Media { get; set; }

    [CliOption("--output-key")]
    public string? OutputKey { get; set; }

    [CliOption("--output-encryption-kms-key-id")]
    public string? OutputEncryptionKmsKeyId { get; set; }

    [CliOption("--kms-encryption-context")]
    public IEnumerable<KeyValue>? KmsEncryptionContext { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--content-identification-type")]
    public string? ContentIdentificationType { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
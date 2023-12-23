using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "start-medical-transcription-job")]
public record AwsTranscribeStartMedicalTranscriptionJobOptions(
[property: CommandSwitch("--medical-transcription-job-name")] string MedicalTranscriptionJobName,
[property: CommandSwitch("--language-code")] string LanguageCode,
[property: CommandSwitch("--output-bucket-name")] string OutputBucketName,
[property: CommandSwitch("--specialty")] string Specialty,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--media-sample-rate-hertz")]
    public int? MediaSampleRateHertz { get; set; }

    [CommandSwitch("--media-format")]
    public string? MediaFormat { get; set; }

    [CommandSwitch("--media")]
    public string? Media { get; set; }

    [CommandSwitch("--output-key")]
    public string? OutputKey { get; set; }

    [CommandSwitch("--output-encryption-kms-key-id")]
    public string? OutputEncryptionKmsKeyId { get; set; }

    [CommandSwitch("--kms-encryption-context")]
    public IEnumerable<KeyValue>? KmsEncryptionContext { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--content-identification-type")]
    public string? ContentIdentificationType { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "start-medical-scribe-job")]
public record AwsTranscribeStartMedicalScribeJobOptions(
[property: CliOption("--medical-scribe-job-name")] string MedicalScribeJobName,
[property: CliOption("--media")] string Media,
[property: CliOption("--output-bucket-name")] string OutputBucketName,
[property: CliOption("--data-access-role-arn")] string DataAccessRoleArn,
[property: CliOption("--settings")] string Settings
) : AwsOptions
{
    [CliOption("--output-encryption-kms-key-id")]
    public string? OutputEncryptionKmsKeyId { get; set; }

    [CliOption("--kms-encryption-context")]
    public IEnumerable<KeyValue>? KmsEncryptionContext { get; set; }

    [CliOption("--channel-definitions")]
    public string[]? ChannelDefinitions { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
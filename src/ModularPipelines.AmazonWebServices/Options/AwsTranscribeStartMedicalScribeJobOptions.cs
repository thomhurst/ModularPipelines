using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "start-medical-scribe-job")]
public record AwsTranscribeStartMedicalScribeJobOptions(
[property: CommandSwitch("--medical-scribe-job-name")] string MedicalScribeJobName,
[property: CommandSwitch("--media")] string Media,
[property: CommandSwitch("--output-bucket-name")] string OutputBucketName,
[property: CommandSwitch("--data-access-role-arn")] string DataAccessRoleArn,
[property: CommandSwitch("--settings")] string Settings
) : AwsOptions
{
    [CommandSwitch("--output-encryption-kms-key-id")]
    public string? OutputEncryptionKmsKeyId { get; set; }

    [CommandSwitch("--kms-encryption-context")]
    public IEnumerable<KeyValue>? KmsEncryptionContext { get; set; }

    [CommandSwitch("--channel-definitions")]
    public string[]? ChannelDefinitions { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
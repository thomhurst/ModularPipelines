using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "start-ml-data-processing-job")]
public record AwsNeptunedataStartMlDataProcessingJobOptions(
[property: CommandSwitch("--input-data-s3-location")] string InputDataS3Location,
[property: CommandSwitch("--processed-data-s3-location")] string ProcessedDataS3Location
) : AwsOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--previous-data-processing-job-id")]
    public string? PreviousDataProcessingJobId { get; set; }

    [CommandSwitch("--sagemaker-iam-role-arn")]
    public string? SagemakerIamRoleArn { get; set; }

    [CommandSwitch("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CommandSwitch("--processing-instance-type")]
    public string? ProcessingInstanceType { get; set; }

    [CommandSwitch("--processing-instance-volume-size-in-gb")]
    public int? ProcessingInstanceVolumeSizeInGb { get; set; }

    [CommandSwitch("--processing-time-out-in-seconds")]
    public int? ProcessingTimeOutInSeconds { get; set; }

    [CommandSwitch("--model-type")]
    public string? ModelType { get; set; }

    [CommandSwitch("--config-file-name")]
    public string? ConfigFileName { get; set; }

    [CommandSwitch("--subnets")]
    public string[]? Subnets { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--volume-encryption-kms-key")]
    public string? VolumeEncryptionKmsKey { get; set; }

    [CommandSwitch("--s3-output-encryption-kms-key")]
    public string? S3OutputEncryptionKmsKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
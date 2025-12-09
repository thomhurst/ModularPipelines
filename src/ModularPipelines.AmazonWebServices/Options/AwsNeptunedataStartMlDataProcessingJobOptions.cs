using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "start-ml-data-processing-job")]
public record AwsNeptunedataStartMlDataProcessingJobOptions(
[property: CliOption("--input-data-s3-location")] string InputDataS3Location,
[property: CliOption("--processed-data-s3-location")] string ProcessedDataS3Location
) : AwsOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--previous-data-processing-job-id")]
    public string? PreviousDataProcessingJobId { get; set; }

    [CliOption("--sagemaker-iam-role-arn")]
    public string? SagemakerIamRoleArn { get; set; }

    [CliOption("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CliOption("--processing-instance-type")]
    public string? ProcessingInstanceType { get; set; }

    [CliOption("--processing-instance-volume-size-in-gb")]
    public int? ProcessingInstanceVolumeSizeInGb { get; set; }

    [CliOption("--processing-time-out-in-seconds")]
    public int? ProcessingTimeOutInSeconds { get; set; }

    [CliOption("--model-type")]
    public string? ModelType { get; set; }

    [CliOption("--config-file-name")]
    public string? ConfigFileName { get; set; }

    [CliOption("--subnets")]
    public string[]? Subnets { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--volume-encryption-kms-key")]
    public string? VolumeEncryptionKmsKey { get; set; }

    [CliOption("--s3-output-encryption-kms-key")]
    public string? S3OutputEncryptionKmsKey { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
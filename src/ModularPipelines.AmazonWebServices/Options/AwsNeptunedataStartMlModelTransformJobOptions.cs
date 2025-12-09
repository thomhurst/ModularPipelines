using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "start-ml-model-transform-job")]
public record AwsNeptunedataStartMlModelTransformJobOptions(
[property: CliOption("--model-transform-output-s3-location")] string ModelTransformOutputS3Location
) : AwsOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--data-processing-job-id")]
    public string? DataProcessingJobId { get; set; }

    [CliOption("--ml-model-training-job-id")]
    public string? MlModelTrainingJobId { get; set; }

    [CliOption("--training-job-name")]
    public string? TrainingJobName { get; set; }

    [CliOption("--sagemaker-iam-role-arn")]
    public string? SagemakerIamRoleArn { get; set; }

    [CliOption("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CliOption("--custom-model-transform-parameters")]
    public string? CustomModelTransformParameters { get; set; }

    [CliOption("--base-processing-instance-type")]
    public string? BaseProcessingInstanceType { get; set; }

    [CliOption("--base-processing-instance-volume-size-in-gb")]
    public int? BaseProcessingInstanceVolumeSizeInGb { get; set; }

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
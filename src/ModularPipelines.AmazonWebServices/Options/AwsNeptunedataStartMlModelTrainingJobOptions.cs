using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "start-ml-model-training-job")]
public record AwsNeptunedataStartMlModelTrainingJobOptions(
[property: CliOption("--data-processing-job-id")] string DataProcessingJobId,
[property: CliOption("--train-model-s3-location")] string TrainModelS3Location
) : AwsOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--previous-model-training-job-id")]
    public string? PreviousModelTrainingJobId { get; set; }

    [CliOption("--sagemaker-iam-role-arn")]
    public string? SagemakerIamRoleArn { get; set; }

    [CliOption("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CliOption("--base-processing-instance-type")]
    public string? BaseProcessingInstanceType { get; set; }

    [CliOption("--training-instance-type")]
    public string? TrainingInstanceType { get; set; }

    [CliOption("--training-instance-volume-size-in-gb")]
    public int? TrainingInstanceVolumeSizeInGb { get; set; }

    [CliOption("--training-time-out-in-seconds")]
    public int? TrainingTimeOutInSeconds { get; set; }

    [CliOption("--max-hpo-number-of-training-jobs")]
    public int? MaxHpoNumberOfTrainingJobs { get; set; }

    [CliOption("--max-hpo-parallel-training-jobs")]
    public int? MaxHpoParallelTrainingJobs { get; set; }

    [CliOption("--subnets")]
    public string[]? Subnets { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--volume-encryption-kms-key")]
    public string? VolumeEncryptionKmsKey { get; set; }

    [CliOption("--s3-output-encryption-kms-key")]
    public string? S3OutputEncryptionKmsKey { get; set; }

    [CliOption("--custom-model-training-parameters")]
    public string? CustomModelTrainingParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
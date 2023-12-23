using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "start-ml-model-training-job")]
public record AwsNeptunedataStartMlModelTrainingJobOptions(
[property: CommandSwitch("--data-processing-job-id")] string DataProcessingJobId,
[property: CommandSwitch("--train-model-s3-location")] string TrainModelS3Location
) : AwsOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--previous-model-training-job-id")]
    public string? PreviousModelTrainingJobId { get; set; }

    [CommandSwitch("--sagemaker-iam-role-arn")]
    public string? SagemakerIamRoleArn { get; set; }

    [CommandSwitch("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CommandSwitch("--base-processing-instance-type")]
    public string? BaseProcessingInstanceType { get; set; }

    [CommandSwitch("--training-instance-type")]
    public string? TrainingInstanceType { get; set; }

    [CommandSwitch("--training-instance-volume-size-in-gb")]
    public int? TrainingInstanceVolumeSizeInGb { get; set; }

    [CommandSwitch("--training-time-out-in-seconds")]
    public int? TrainingTimeOutInSeconds { get; set; }

    [CommandSwitch("--max-hpo-number-of-training-jobs")]
    public int? MaxHpoNumberOfTrainingJobs { get; set; }

    [CommandSwitch("--max-hpo-parallel-training-jobs")]
    public int? MaxHpoParallelTrainingJobs { get; set; }

    [CommandSwitch("--subnets")]
    public string[]? Subnets { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--volume-encryption-kms-key")]
    public string? VolumeEncryptionKmsKey { get; set; }

    [CommandSwitch("--s3-output-encryption-kms-key")]
    public string? S3OutputEncryptionKmsKey { get; set; }

    [CommandSwitch("--custom-model-training-parameters")]
    public string? CustomModelTrainingParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "start-ml-model-transform-job")]
public record AwsNeptunedataStartMlModelTransformJobOptions(
[property: CommandSwitch("--model-transform-output-s3-location")] string ModelTransformOutputS3Location
) : AwsOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--data-processing-job-id")]
    public string? DataProcessingJobId { get; set; }

    [CommandSwitch("--ml-model-training-job-id")]
    public string? MlModelTrainingJobId { get; set; }

    [CommandSwitch("--training-job-name")]
    public string? TrainingJobName { get; set; }

    [CommandSwitch("--sagemaker-iam-role-arn")]
    public string? SagemakerIamRoleArn { get; set; }

    [CommandSwitch("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CommandSwitch("--custom-model-transform-parameters")]
    public string? CustomModelTransformParameters { get; set; }

    [CommandSwitch("--base-processing-instance-type")]
    public string? BaseProcessingInstanceType { get; set; }

    [CommandSwitch("--base-processing-instance-volume-size-in-gb")]
    public int? BaseProcessingInstanceVolumeSizeInGb { get; set; }

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
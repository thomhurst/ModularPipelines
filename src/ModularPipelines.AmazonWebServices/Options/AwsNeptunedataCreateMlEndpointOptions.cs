using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "create-ml-endpoint")]
public record AwsNeptunedataCreateMlEndpointOptions : AwsOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--ml-model-training-job-id")]
    public string? MlModelTrainingJobId { get; set; }

    [CommandSwitch("--ml-model-transform-job-id")]
    public string? MlModelTransformJobId { get; set; }

    [CommandSwitch("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CommandSwitch("--model-name")]
    public string? ModelName { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--volume-encryption-kms-key")]
    public string? VolumeEncryptionKmsKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "create-ml-endpoint")]
public record AwsNeptunedataCreateMlEndpointOptions : AwsOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--ml-model-training-job-id")]
    public string? MlModelTrainingJobId { get; set; }

    [CliOption("--ml-model-transform-job-id")]
    public string? MlModelTransformJobId { get; set; }

    [CliOption("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CliOption("--model-name")]
    public string? ModelName { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--volume-encryption-kms-key")]
    public string? VolumeEncryptionKmsKey { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
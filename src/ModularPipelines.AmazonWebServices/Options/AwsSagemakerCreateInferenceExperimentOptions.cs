using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-inference-experiment")]
public record AwsSagemakerCreateInferenceExperimentOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--model-variants")] string[] ModelVariants,
[property: CliOption("--shadow-mode-config")] string ShadowModeConfig
) : AwsOptions
{
    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--data-storage-config")]
    public string? DataStorageConfig { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
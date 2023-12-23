using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-inference-experiment")]
public record AwsSagemakerCreateInferenceExperimentOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--model-variants")] string[] ModelVariants,
[property: CommandSwitch("--shadow-mode-config")] string ShadowModeConfig
) : AwsOptions
{
    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--data-storage-config")]
    public string? DataStorageConfig { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
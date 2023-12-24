using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock", "create-model-customization-job")]
public record AwsBedrockCreateModelCustomizationJobOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--custom-model-name")] string CustomModelName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--base-model-identifier")] string BaseModelIdentifier,
[property: CommandSwitch("--training-data-config")] string TrainingDataConfig,
[property: CommandSwitch("--output-data-config")] string OutputDataConfig,
[property: CommandSwitch("--hyper-parameters")] IEnumerable<KeyValue> HyperParameters
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--customization-type")]
    public string? CustomizationType { get; set; }

    [CommandSwitch("--custom-model-kms-key-id")]
    public string? CustomModelKmsKeyId { get; set; }

    [CommandSwitch("--job-tags")]
    public string[]? JobTags { get; set; }

    [CommandSwitch("--custom-model-tags")]
    public string[]? CustomModelTags { get; set; }

    [CommandSwitch("--validation-data-config")]
    public string? ValidationDataConfig { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock", "create-model-customization-job")]
public record AwsBedrockCreateModelCustomizationJobOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--custom-model-name")] string CustomModelName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--base-model-identifier")] string BaseModelIdentifier,
[property: CliOption("--training-data-config")] string TrainingDataConfig,
[property: CliOption("--output-data-config")] string OutputDataConfig,
[property: CliOption("--hyper-parameters")] IEnumerable<KeyValue> HyperParameters
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--customization-type")]
    public string? CustomizationType { get; set; }

    [CliOption("--custom-model-kms-key-id")]
    public string? CustomModelKmsKeyId { get; set; }

    [CliOption("--job-tags")]
    public string[]? JobTags { get; set; }

    [CliOption("--custom-model-tags")]
    public string[]? CustomModelTags { get; set; }

    [CliOption("--validation-data-config")]
    public string? ValidationDataConfig { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
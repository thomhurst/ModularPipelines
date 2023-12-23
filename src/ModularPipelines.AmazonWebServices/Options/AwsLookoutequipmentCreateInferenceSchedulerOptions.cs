using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "create-inference-scheduler")]
public record AwsLookoutequipmentCreateInferenceSchedulerOptions(
[property: CommandSwitch("--model-name")] string ModelName,
[property: CommandSwitch("--inference-scheduler-name")] string InferenceSchedulerName,
[property: CommandSwitch("--data-upload-frequency")] string DataUploadFrequency,
[property: CommandSwitch("--data-input-configuration")] string DataInputConfiguration,
[property: CommandSwitch("--data-output-configuration")] string DataOutputConfiguration,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--data-delay-offset-in-minutes")]
    public long? DataDelayOffsetInMinutes { get; set; }

    [CommandSwitch("--server-side-kms-key-id")]
    public string? ServerSideKmsKeyId { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
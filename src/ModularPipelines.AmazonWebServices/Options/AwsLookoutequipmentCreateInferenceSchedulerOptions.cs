using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "create-inference-scheduler")]
public record AwsLookoutequipmentCreateInferenceSchedulerOptions(
[property: CliOption("--model-name")] string ModelName,
[property: CliOption("--inference-scheduler-name")] string InferenceSchedulerName,
[property: CliOption("--data-upload-frequency")] string DataUploadFrequency,
[property: CliOption("--data-input-configuration")] string DataInputConfiguration,
[property: CliOption("--data-output-configuration")] string DataOutputConfiguration,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--data-delay-offset-in-minutes")]
    public long? DataDelayOffsetInMinutes { get; set; }

    [CliOption("--server-side-kms-key-id")]
    public string? ServerSideKmsKeyId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
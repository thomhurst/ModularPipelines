using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "update-inference-scheduler")]
public record AwsLookoutequipmentUpdateInferenceSchedulerOptions(
[property: CommandSwitch("--inference-scheduler-name")] string InferenceSchedulerName
) : AwsOptions
{
    [CommandSwitch("--data-delay-offset-in-minutes")]
    public long? DataDelayOffsetInMinutes { get; set; }

    [CommandSwitch("--data-upload-frequency")]
    public string? DataUploadFrequency { get; set; }

    [CommandSwitch("--data-input-configuration")]
    public string? DataInputConfiguration { get; set; }

    [CommandSwitch("--data-output-configuration")]
    public string? DataOutputConfiguration { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
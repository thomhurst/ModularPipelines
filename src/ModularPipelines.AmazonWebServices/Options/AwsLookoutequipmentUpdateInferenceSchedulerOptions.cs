using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "update-inference-scheduler")]
public record AwsLookoutequipmentUpdateInferenceSchedulerOptions(
[property: CliOption("--inference-scheduler-name")] string InferenceSchedulerName
) : AwsOptions
{
    [CliOption("--data-delay-offset-in-minutes")]
    public long? DataDelayOffsetInMinutes { get; set; }

    [CliOption("--data-upload-frequency")]
    public string? DataUploadFrequency { get; set; }

    [CliOption("--data-input-configuration")]
    public string? DataInputConfiguration { get; set; }

    [CliOption("--data-output-configuration")]
    public string? DataOutputConfiguration { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
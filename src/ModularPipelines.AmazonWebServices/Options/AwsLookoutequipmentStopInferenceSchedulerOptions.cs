using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "stop-inference-scheduler")]
public record AwsLookoutequipmentStopInferenceSchedulerOptions(
[property: CommandSwitch("--inference-scheduler-name")] string InferenceSchedulerName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
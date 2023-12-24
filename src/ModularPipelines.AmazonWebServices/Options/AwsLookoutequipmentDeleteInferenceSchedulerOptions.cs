using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "delete-inference-scheduler")]
public record AwsLookoutequipmentDeleteInferenceSchedulerOptions(
[property: CommandSwitch("--inference-scheduler-name")] string InferenceSchedulerName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
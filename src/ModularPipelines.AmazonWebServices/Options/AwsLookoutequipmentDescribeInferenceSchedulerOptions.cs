using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "describe-inference-scheduler")]
public record AwsLookoutequipmentDescribeInferenceSchedulerOptions(
[property: CommandSwitch("--inference-scheduler-name")] string InferenceSchedulerName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
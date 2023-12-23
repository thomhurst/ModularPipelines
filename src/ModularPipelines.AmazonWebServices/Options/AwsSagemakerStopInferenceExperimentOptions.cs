using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "stop-inference-experiment")]
public record AwsSagemakerStopInferenceExperimentOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--model-variant-actions")] IEnumerable<KeyValue> ModelVariantActions
) : AwsOptions
{
    [CommandSwitch("--desired-model-variants")]
    public string[]? DesiredModelVariants { get; set; }

    [CommandSwitch("--desired-state")]
    public string? DesiredState { get; set; }

    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
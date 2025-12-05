using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "stop-inference-experiment")]
public record AwsSagemakerStopInferenceExperimentOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--model-variant-actions")] IEnumerable<KeyValue> ModelVariantActions
) : AwsOptions
{
    [CliOption("--desired-model-variants")]
    public string[]? DesiredModelVariants { get; set; }

    [CliOption("--desired-state")]
    public string? DesiredState { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
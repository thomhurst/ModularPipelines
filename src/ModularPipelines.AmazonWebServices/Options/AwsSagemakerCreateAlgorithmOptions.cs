using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-algorithm")]
public record AwsSagemakerCreateAlgorithmOptions(
[property: CommandSwitch("--algorithm-name")] string AlgorithmName,
[property: CommandSwitch("--training-specification")] string TrainingSpecification
) : AwsOptions
{
    [CommandSwitch("--algorithm-description")]
    public string? AlgorithmDescription { get; set; }

    [CommandSwitch("--inference-specification")]
    public string? InferenceSpecification { get; set; }

    [CommandSwitch("--validation-specification")]
    public string? ValidationSpecification { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
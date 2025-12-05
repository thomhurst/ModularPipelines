using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-algorithm")]
public record AwsSagemakerCreateAlgorithmOptions(
[property: CliOption("--algorithm-name")] string AlgorithmName,
[property: CliOption("--training-specification")] string TrainingSpecification
) : AwsOptions
{
    [CliOption("--algorithm-description")]
    public string? AlgorithmDescription { get; set; }

    [CliOption("--inference-specification")]
    public string? InferenceSpecification { get; set; }

    [CliOption("--validation-specification")]
    public string? ValidationSpecification { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-inference-experiment")]
public record AwsSagemakerUpdateInferenceExperimentOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--model-variants")]
    public string[]? ModelVariants { get; set; }

    [CliOption("--data-storage-config")]
    public string? DataStorageConfig { get; set; }

    [CliOption("--shadow-mode-config")]
    public string? ShadowModeConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
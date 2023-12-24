using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-inference-experiment")]
public record AwsSagemakerUpdateInferenceExperimentOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--model-variants")]
    public string[]? ModelVariants { get; set; }

    [CommandSwitch("--data-storage-config")]
    public string? DataStorageConfig { get; set; }

    [CommandSwitch("--shadow-mode-config")]
    public string? ShadowModeConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
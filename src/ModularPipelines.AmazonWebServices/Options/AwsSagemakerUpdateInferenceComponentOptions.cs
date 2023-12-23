using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-inference-component")]
public record AwsSagemakerUpdateInferenceComponentOptions(
[property: CommandSwitch("--inference-component-name")] string InferenceComponentName
) : AwsOptions
{
    [CommandSwitch("--specification")]
    public string? Specification { get; set; }

    [CommandSwitch("--runtime-config")]
    public string? RuntimeConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
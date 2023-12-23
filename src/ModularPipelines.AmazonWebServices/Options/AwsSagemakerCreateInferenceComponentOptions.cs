using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-inference-component")]
public record AwsSagemakerCreateInferenceComponentOptions(
[property: CommandSwitch("--inference-component-name")] string InferenceComponentName,
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--variant-name")] string VariantName,
[property: CommandSwitch("--specification")] string Specification,
[property: CommandSwitch("--runtime-config")] string RuntimeConfig
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
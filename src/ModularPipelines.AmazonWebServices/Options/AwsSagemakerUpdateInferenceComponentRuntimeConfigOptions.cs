using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-inference-component-runtime-config")]
public record AwsSagemakerUpdateInferenceComponentRuntimeConfigOptions(
[property: CommandSwitch("--inference-component-name")] string InferenceComponentName,
[property: CommandSwitch("--desired-runtime-config")] string DesiredRuntimeConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
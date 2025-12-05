using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-inference-component-runtime-config")]
public record AwsSagemakerUpdateInferenceComponentRuntimeConfigOptions(
[property: CliOption("--inference-component-name")] string InferenceComponentName,
[property: CliOption("--desired-runtime-config")] string DesiredRuntimeConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
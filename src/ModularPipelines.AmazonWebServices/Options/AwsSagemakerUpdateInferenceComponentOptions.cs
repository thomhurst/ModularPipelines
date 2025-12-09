using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-inference-component")]
public record AwsSagemakerUpdateInferenceComponentOptions(
[property: CliOption("--inference-component-name")] string InferenceComponentName
) : AwsOptions
{
    [CliOption("--specification")]
    public string? Specification { get; set; }

    [CliOption("--runtime-config")]
    public string? RuntimeConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
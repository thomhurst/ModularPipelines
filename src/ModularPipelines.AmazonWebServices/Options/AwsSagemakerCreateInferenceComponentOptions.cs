using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-inference-component")]
public record AwsSagemakerCreateInferenceComponentOptions(
[property: CliOption("--inference-component-name")] string InferenceComponentName,
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--variant-name")] string VariantName,
[property: CliOption("--specification")] string Specification,
[property: CliOption("--runtime-config")] string RuntimeConfig
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
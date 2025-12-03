using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-runtime", "invoke-endpoint")]
public record AwsSagemakerRuntimeInvokeEndpointOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--body")] string Body
) : AwsOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--accept")]
    public string? Accept { get; set; }

    [CliOption("--custom-attributes")]
    public string? CustomAttributes { get; set; }

    [CliOption("--target-model")]
    public string? TargetModel { get; set; }

    [CliOption("--target-variant")]
    public string? TargetVariant { get; set; }

    [CliOption("--target-container-hostname")]
    public string? TargetContainerHostname { get; set; }

    [CliOption("--inference-id")]
    public string? InferenceId { get; set; }

    [CliOption("--enable-explanations")]
    public string? EnableExplanations { get; set; }

    [CliOption("--inference-component-name")]
    public string? InferenceComponentName { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-runtime", "invoke-endpoint")]
public record AwsSagemakerRuntimeInvokeEndpointOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--body")] string Body
) : AwsOptions
{
    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--accept")]
    public string? Accept { get; set; }

    [CommandSwitch("--custom-attributes")]
    public string? CustomAttributes { get; set; }

    [CommandSwitch("--target-model")]
    public string? TargetModel { get; set; }

    [CommandSwitch("--target-variant")]
    public string? TargetVariant { get; set; }

    [CommandSwitch("--target-container-hostname")]
    public string? TargetContainerHostname { get; set; }

    [CommandSwitch("--inference-id")]
    public string? InferenceId { get; set; }

    [CommandSwitch("--enable-explanations")]
    public string? EnableExplanations { get; set; }

    [CommandSwitch("--inference-component-name")]
    public string? InferenceComponentName { get; set; }
}
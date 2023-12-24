using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-runtime", "invoke-endpoint-async")]
public record AwsSagemakerRuntimeInvokeEndpointAsyncOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--input-location")] string InputLocation
) : AwsOptions
{
    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--accept")]
    public string? Accept { get; set; }

    [CommandSwitch("--custom-attributes")]
    public string? CustomAttributes { get; set; }

    [CommandSwitch("--inference-id")]
    public string? InferenceId { get; set; }

    [CommandSwitch("--request-ttl-seconds")]
    public int? RequestTtlSeconds { get; set; }

    [CommandSwitch("--invocation-timeout-seconds")]
    public int? InvocationTimeoutSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-runtime", "invoke-endpoint-async")]
public record AwsSagemakerRuntimeInvokeEndpointAsyncOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--input-location")] string InputLocation
) : AwsOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--accept")]
    public string? Accept { get; set; }

    [CliOption("--custom-attributes")]
    public string? CustomAttributes { get; set; }

    [CliOption("--inference-id")]
    public string? InferenceId { get; set; }

    [CliOption("--request-ttl-seconds")]
    public int? RequestTtlSeconds { get; set; }

    [CliOption("--invocation-timeout-seconds")]
    public int? InvocationTimeoutSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
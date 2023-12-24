using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "test-invoke-method")]
public record AwsApigatewayTestInvokeMethodOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--http-method")] string HttpMethod
) : AwsOptions
{
    [CommandSwitch("--path-with-query-string")]
    public string? PathWithQueryString { get; set; }

    [CommandSwitch("--body")]
    public string? Body { get; set; }

    [CommandSwitch("--headers")]
    public IEnumerable<KeyValue>? Headers { get; set; }

    [CommandSwitch("--multi-value-headers")]
    public IEnumerable<KeyValue>? MultiValueHeaders { get; set; }

    [CommandSwitch("--client-certificate-id")]
    public string? ClientCertificateId { get; set; }

    [CommandSwitch("--stage-variables")]
    public IEnumerable<KeyValue>? StageVariables { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "put-integration")]
public record AwsApigatewayPutIntegrationOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--http-method")] string HttpMethod,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--integration-http-method")]
    public string? IntegrationHttpMethod { get; set; }

    [CommandSwitch("--uri")]
    public string? Uri { get; set; }

    [CommandSwitch("--connection-type")]
    public string? ConnectionType { get; set; }

    [CommandSwitch("--connection-id")]
    public string? ConnectionId { get; set; }

    [CommandSwitch("--credentials")]
    public string? Credentials { get; set; }

    [CommandSwitch("--request-parameters")]
    public IEnumerable<KeyValue>? RequestParameters { get; set; }

    [CommandSwitch("--request-templates")]
    public IEnumerable<KeyValue>? RequestTemplates { get; set; }

    [CommandSwitch("--passthrough-behavior")]
    public string? PassthroughBehavior { get; set; }

    [CommandSwitch("--cache-namespace")]
    public string? CacheNamespace { get; set; }

    [CommandSwitch("--cache-key-parameters")]
    public string[]? CacheKeyParameters { get; set; }

    [CommandSwitch("--content-handling")]
    public string? ContentHandling { get; set; }

    [CommandSwitch("--timeout-in-millis")]
    public int? TimeoutInMillis { get; set; }

    [CommandSwitch("--tls-config")]
    public string? TlsConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "put-integration")]
public record AwsApigatewayPutIntegrationOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--http-method")] string HttpMethod,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--integration-http-method")]
    public string? IntegrationHttpMethod { get; set; }

    [CliOption("--uri")]
    public string? Uri { get; set; }

    [CliOption("--connection-type")]
    public string? ConnectionType { get; set; }

    [CliOption("--connection-id")]
    public string? ConnectionId { get; set; }

    [CliOption("--credentials")]
    public string? Credentials { get; set; }

    [CliOption("--request-parameters")]
    public IEnumerable<KeyValue>? RequestParameters { get; set; }

    [CliOption("--request-templates")]
    public IEnumerable<KeyValue>? RequestTemplates { get; set; }

    [CliOption("--passthrough-behavior")]
    public string? PassthroughBehavior { get; set; }

    [CliOption("--cache-namespace")]
    public string? CacheNamespace { get; set; }

    [CliOption("--cache-key-parameters")]
    public string[]? CacheKeyParameters { get; set; }

    [CliOption("--content-handling")]
    public string? ContentHandling { get; set; }

    [CliOption("--timeout-in-millis")]
    public int? TimeoutInMillis { get; set; }

    [CliOption("--tls-config")]
    public string? TlsConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
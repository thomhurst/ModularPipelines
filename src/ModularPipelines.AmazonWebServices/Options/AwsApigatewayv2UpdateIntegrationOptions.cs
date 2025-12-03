using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "update-integration")]
public record AwsApigatewayv2UpdateIntegrationOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--integration-id")] string IntegrationId
) : AwsOptions
{
    [CliOption("--connection-id")]
    public string? ConnectionId { get; set; }

    [CliOption("--connection-type")]
    public string? ConnectionType { get; set; }

    [CliOption("--content-handling-strategy")]
    public string? ContentHandlingStrategy { get; set; }

    [CliOption("--credentials-arn")]
    public string? CredentialsArn { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--integration-method")]
    public string? IntegrationMethod { get; set; }

    [CliOption("--integration-subtype")]
    public string? IntegrationSubtype { get; set; }

    [CliOption("--integration-type")]
    public string? IntegrationType { get; set; }

    [CliOption("--integration-uri")]
    public string? IntegrationUri { get; set; }

    [CliOption("--passthrough-behavior")]
    public string? PassthroughBehavior { get; set; }

    [CliOption("--payload-format-version")]
    public string? PayloadFormatVersion { get; set; }

    [CliOption("--request-parameters")]
    public IEnumerable<KeyValue>? RequestParameters { get; set; }

    [CliOption("--request-templates")]
    public IEnumerable<KeyValue>? RequestTemplates { get; set; }

    [CliOption("--response-parameters")]
    public IEnumerable<KeyValue>? ResponseParameters { get; set; }

    [CliOption("--template-selection-expression")]
    public string? TemplateSelectionExpression { get; set; }

    [CliOption("--timeout-in-millis")]
    public int? TimeoutInMillis { get; set; }

    [CliOption("--tls-config")]
    public string? TlsConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
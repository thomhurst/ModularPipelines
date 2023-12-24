using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "create-integration")]
public record AwsApigatewayv2CreateIntegrationOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--integration-type")] string IntegrationType
) : AwsOptions
{
    [CommandSwitch("--connection-id")]
    public string? ConnectionId { get; set; }

    [CommandSwitch("--connection-type")]
    public string? ConnectionType { get; set; }

    [CommandSwitch("--content-handling-strategy")]
    public string? ContentHandlingStrategy { get; set; }

    [CommandSwitch("--credentials-arn")]
    public string? CredentialsArn { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--integration-method")]
    public string? IntegrationMethod { get; set; }

    [CommandSwitch("--integration-subtype")]
    public string? IntegrationSubtype { get; set; }

    [CommandSwitch("--integration-uri")]
    public string? IntegrationUri { get; set; }

    [CommandSwitch("--passthrough-behavior")]
    public string? PassthroughBehavior { get; set; }

    [CommandSwitch("--payload-format-version")]
    public string? PayloadFormatVersion { get; set; }

    [CommandSwitch("--request-parameters")]
    public IEnumerable<KeyValue>? RequestParameters { get; set; }

    [CommandSwitch("--request-templates")]
    public IEnumerable<KeyValue>? RequestTemplates { get; set; }

    [CommandSwitch("--response-parameters")]
    public IEnumerable<KeyValue>? ResponseParameters { get; set; }

    [CommandSwitch("--template-selection-expression")]
    public string? TemplateSelectionExpression { get; set; }

    [CommandSwitch("--timeout-in-millis")]
    public int? TimeoutInMillis { get; set; }

    [CommandSwitch("--tls-config")]
    public string? TlsConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
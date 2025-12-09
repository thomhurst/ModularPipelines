using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "create-integration-response")]
public record AwsApigatewayv2CreateIntegrationResponseOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--integration-id")] string IntegrationId,
[property: CliOption("--integration-response-key")] string IntegrationResponseKey
) : AwsOptions
{
    [CliOption("--content-handling-strategy")]
    public string? ContentHandlingStrategy { get; set; }

    [CliOption("--response-parameters")]
    public IEnumerable<KeyValue>? ResponseParameters { get; set; }

    [CliOption("--response-templates")]
    public IEnumerable<KeyValue>? ResponseTemplates { get; set; }

    [CliOption("--template-selection-expression")]
    public string? TemplateSelectionExpression { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
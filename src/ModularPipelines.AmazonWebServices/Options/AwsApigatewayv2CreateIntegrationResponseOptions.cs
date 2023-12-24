using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "create-integration-response")]
public record AwsApigatewayv2CreateIntegrationResponseOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--integration-id")] string IntegrationId,
[property: CommandSwitch("--integration-response-key")] string IntegrationResponseKey
) : AwsOptions
{
    [CommandSwitch("--content-handling-strategy")]
    public string? ContentHandlingStrategy { get; set; }

    [CommandSwitch("--response-parameters")]
    public IEnumerable<KeyValue>? ResponseParameters { get; set; }

    [CommandSwitch("--response-templates")]
    public IEnumerable<KeyValue>? ResponseTemplates { get; set; }

    [CommandSwitch("--template-selection-expression")]
    public string? TemplateSelectionExpression { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
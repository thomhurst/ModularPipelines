using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "put-integration-response")]
public record AwsApigatewayPutIntegrationResponseOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--http-method")] string HttpMethod,
[property: CommandSwitch("--status-code")] string StatusCode
) : AwsOptions
{
    [CommandSwitch("--selection-pattern")]
    public string? SelectionPattern { get; set; }

    [CommandSwitch("--response-parameters")]
    public IEnumerable<KeyValue>? ResponseParameters { get; set; }

    [CommandSwitch("--response-templates")]
    public IEnumerable<KeyValue>? ResponseTemplates { get; set; }

    [CommandSwitch("--content-handling")]
    public string? ContentHandling { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
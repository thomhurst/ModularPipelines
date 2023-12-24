using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "put-gateway-response")]
public record AwsApigatewayPutGatewayResponseOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--response-type")] string ResponseType
) : AwsOptions
{
    [CommandSwitch("--status-code")]
    public string? StatusCode { get; set; }

    [CommandSwitch("--response-parameters")]
    public IEnumerable<KeyValue>? ResponseParameters { get; set; }

    [CommandSwitch("--response-templates")]
    public IEnumerable<KeyValue>? ResponseTemplates { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
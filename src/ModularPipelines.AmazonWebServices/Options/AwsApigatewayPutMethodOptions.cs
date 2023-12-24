using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "put-method")]
public record AwsApigatewayPutMethodOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--http-method")] string HttpMethod,
[property: CommandSwitch("--authorization-type")] string AuthorizationType
) : AwsOptions
{
    [CommandSwitch("--authorizer-id")]
    public string? AuthorizerId { get; set; }

    [CommandSwitch("--operation-name")]
    public string? OperationName { get; set; }

    [CommandSwitch("--request-parameters")]
    public IEnumerable<KeyValue>? RequestParameters { get; set; }

    [CommandSwitch("--request-models")]
    public IEnumerable<KeyValue>? RequestModels { get; set; }

    [CommandSwitch("--request-validator-id")]
    public string? RequestValidatorId { get; set; }

    [CommandSwitch("--authorization-scopes")]
    public string[]? AuthorizationScopes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
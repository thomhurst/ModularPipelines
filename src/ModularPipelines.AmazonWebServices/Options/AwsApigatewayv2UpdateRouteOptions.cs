using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "update-route")]
public record AwsApigatewayv2UpdateRouteOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--route-id")] string RouteId
) : AwsOptions
{
    [CommandSwitch("--authorization-scopes")]
    public string[]? AuthorizationScopes { get; set; }

    [CommandSwitch("--authorization-type")]
    public string? AuthorizationType { get; set; }

    [CommandSwitch("--authorizer-id")]
    public string? AuthorizerId { get; set; }

    [CommandSwitch("--model-selection-expression")]
    public string? ModelSelectionExpression { get; set; }

    [CommandSwitch("--operation-name")]
    public string? OperationName { get; set; }

    [CommandSwitch("--request-models")]
    public IEnumerable<KeyValue>? RequestModels { get; set; }

    [CommandSwitch("--request-parameters")]
    public IEnumerable<KeyValue>? RequestParameters { get; set; }

    [CommandSwitch("--route-key")]
    public string? RouteKey { get; set; }

    [CommandSwitch("--route-response-selection-expression")]
    public string? RouteResponseSelectionExpression { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
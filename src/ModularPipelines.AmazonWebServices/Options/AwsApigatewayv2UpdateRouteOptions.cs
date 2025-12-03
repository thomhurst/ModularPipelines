using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "update-route")]
public record AwsApigatewayv2UpdateRouteOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--route-id")] string RouteId
) : AwsOptions
{
    [CliOption("--authorization-scopes")]
    public string[]? AuthorizationScopes { get; set; }

    [CliOption("--authorization-type")]
    public string? AuthorizationType { get; set; }

    [CliOption("--authorizer-id")]
    public string? AuthorizerId { get; set; }

    [CliOption("--model-selection-expression")]
    public string? ModelSelectionExpression { get; set; }

    [CliOption("--operation-name")]
    public string? OperationName { get; set; }

    [CliOption("--request-models")]
    public IEnumerable<KeyValue>? RequestModels { get; set; }

    [CliOption("--request-parameters")]
    public IEnumerable<KeyValue>? RequestParameters { get; set; }

    [CliOption("--route-key")]
    public string? RouteKey { get; set; }

    [CliOption("--route-response-selection-expression")]
    public string? RouteResponseSelectionExpression { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
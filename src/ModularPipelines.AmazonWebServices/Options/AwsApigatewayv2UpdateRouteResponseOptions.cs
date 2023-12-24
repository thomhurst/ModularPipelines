using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "update-route-response")]
public record AwsApigatewayv2UpdateRouteResponseOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--route-id")] string RouteId,
[property: CommandSwitch("--route-response-id")] string RouteResponseId
) : AwsOptions
{
    [CommandSwitch("--model-selection-expression")]
    public string? ModelSelectionExpression { get; set; }

    [CommandSwitch("--response-models")]
    public IEnumerable<KeyValue>? ResponseModels { get; set; }

    [CommandSwitch("--response-parameters")]
    public IEnumerable<KeyValue>? ResponseParameters { get; set; }

    [CommandSwitch("--route-response-key")]
    public string? RouteResponseKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
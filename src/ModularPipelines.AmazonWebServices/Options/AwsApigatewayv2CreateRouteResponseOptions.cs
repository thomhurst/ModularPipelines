using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "create-route-response")]
public record AwsApigatewayv2CreateRouteResponseOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--route-id")] string RouteId,
[property: CommandSwitch("--route-response-key")] string RouteResponseKey
) : AwsOptions
{
    [CommandSwitch("--model-selection-expression")]
    public string? ModelSelectionExpression { get; set; }

    [CommandSwitch("--response-models")]
    public IEnumerable<KeyValue>? ResponseModels { get; set; }

    [CommandSwitch("--response-parameters")]
    public IEnumerable<KeyValue>? ResponseParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
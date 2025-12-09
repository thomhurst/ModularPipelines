using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "create-route-response")]
public record AwsApigatewayv2CreateRouteResponseOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--route-id")] string RouteId,
[property: CliOption("--route-response-key")] string RouteResponseKey
) : AwsOptions
{
    [CliOption("--model-selection-expression")]
    public string? ModelSelectionExpression { get; set; }

    [CliOption("--response-models")]
    public IEnumerable<KeyValue>? ResponseModels { get; set; }

    [CliOption("--response-parameters")]
    public IEnumerable<KeyValue>? ResponseParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
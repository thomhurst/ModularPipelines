using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "delete-route-response")]
public record AwsApigatewayv2DeleteRouteResponseOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--route-id")] string RouteId,
[property: CommandSwitch("--route-response-id")] string RouteResponseId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
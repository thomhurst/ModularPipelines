using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "delete-route-request-parameter")]
public record AwsApigatewayv2DeleteRouteRequestParameterOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--request-parameter-key")] string RequestParameterKey,
[property: CommandSwitch("--route-id")] string RouteId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}